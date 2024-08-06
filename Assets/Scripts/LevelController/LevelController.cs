using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public ObjectGenerator ObjectGenerator { get; private set; } = new();
    public RoadGenerator RoadGenerator { get; private set; } = new();
    public RoadGenerator DecorationGenerator { get; private set; } = new();

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject CameraPrefab;
    [SerializeField] private GameObject DisableZonePrefab;
    [SerializeField] private GameObject HUDPrefab;

    private GameObject player;
    private GameObject playerCamera;
    private PlayerData playerData;
    private HUDUpdate playerHUD;

    private SwipeController swipeController = new();
    private MovementController movementController = new();

    private ObjectsPool objectsPool = new();
    private Coroutine objectsSpawnRoutine;
    private Coroutine boostersSpawnRoutine;

    private const float SPEED_MODIFY_MULTIPLIER = 0.075f;
    private const float SPAWN_MODIFY_MULTIPLIER = 0.025f;

    private void Awake()
    {
        var levelData = LevelsConfig.Instance.Levels[0];

        RoadGenerator.Initialize(levelData.RoadParts);
        DecorationGenerator.Initialize(levelData.DecorationParts);
        ObjectGenerator.Initialize(levelData.Obstacles, levelData.Collectables);

        CreatePlayer();
        CreateCamera();
        CreateHUD();
        CreateDisableZone();

        swipeController.SetPlayerController(player.GetComponent<PlayerController>());
        movementController.SetInstance();
        PlayerDataUpdateSubscribes();

        RoadGenerator.SetContainer(CreateContainer("RoadPartsContainer", new(0, 0, -15)));
        ObjectGenerator.SetContainers(CreateContainer("ObstacleContainer"), CreateContainer("CollectableContainer", new(0, 1, 0)));
        ObjectGenerator.SetObjectPool(objectsPool);
        DecorationGenerator.SetContainer(CreateContainer("DecorationContainer", new(0, 0, -15)));
    }

    private void Start()
    {
        RoadGenerator.CreateFullRoad(40);
        DecorationGenerator.CreateFullRoad(40);
        objectsSpawnRoutine = StartCoroutine(ObjectGenerator.SpawnObjects());
        boostersSpawnRoutine = StartCoroutine(ObjectGenerator.SpawnBoosters());
    }

    private void Update()
    {
        swipeController.UpdateSwipes();
        movementController.Update();
    }

    private void FixedUpdate()
    {
        movementController.FixedUpdate();
    }
    private void CreatePlayer()
    {
        player = Instantiate(PlayerPrefab);
        player.gameObject.name = "Player";
    }

    private void CreateCamera()
    {
        playerCamera = Instantiate(CameraPrefab);
        playerCamera.name = "Camera";
        CinemachineFreeLook freeLookCamera = playerCamera.GetComponentInChildren<CinemachineFreeLook>();
        freeLookCamera.Follow = player.transform;
        freeLookCamera.LookAt = player.GetComponent<PlayerController>().CameraLookAt;
    }
    private void CreateHUD()
    {
        playerHUD = Instantiate(HUDPrefab).GetComponent<HUDUpdate>();
        playerHUD.gameObject.name = "HUD";
    }

    private Transform CreateContainer(string name)
    {
        GameObject container = new(name);
        return container.transform;
    }

    private Transform CreateContainer(string name, Vector3 newPosition)
    {
        GameObject container = new(name);
        container.transform.position = newPosition;
        return container.transform;
    }


    private void CreateDisableZone()
    {
        GameObject DisableZone = Instantiate(DisableZonePrefab);
        DisableZone.name = "DisableZone";
    }

    private void PlayerDataUpdateSubscribes()
    {
        playerData = player.GetComponent<PlayerData>();
        playerData.UpdateStreak += ChangeSpeed;
        playerData.UpdateStreak += ChangeSpawnTime;
        playerData.GameOver += GameOver;

        //HUD updates
        playerData.UpdatePlayerLifes += playerHUD.UpdateLifes;
        playerData.UpdatePlayerScore += playerHUD.UpdateScore;
        playerData.UpdateStreak += playerHUD.UpdateStreak;
        playerData.UpdateFearEssence += playerHUD.UpdateFearEssence;
        playerData.UpdateBoosterIcon += playerHUD.UpdateBoosterIcon;

    }

    private void ChangeSpeed(int streak)
    {
        if (playerData.IsSlowMotion.Status)
            movementController.ChangeSpeedModify(streak != 0 ? streak * SPEED_MODIFY_MULTIPLIER / 2 : 0);
        else
            movementController.ChangeSpeedModify(streak != 0 ? streak * SPEED_MODIFY_MULTIPLIER : 0);
    }

    private void ChangeSpawnTime(int streak)
    {
        if (playerData.IsSlowMotion.Status)
            ObjectGenerator.ChangeSpawnTime(streak != 0 ? streak * SPAWN_MODIFY_MULTIPLIER / 2 : 0);
        else
            ObjectGenerator.ChangeSpawnTime(streak != 0 ? streak * SPAWN_MODIFY_MULTIPLIER : 0);
    }

    private void GameOver()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
