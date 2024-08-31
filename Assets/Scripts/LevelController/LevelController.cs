using Cinemachine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public ObjectGenerator ObjectGenerator { get; private set; } = new();
    public RoadGenerator RoadGenerator { get; private set; } = new();
    public RoadGenerator DecorationGenerator { get; private set; } = new();

    private ItemController itemController = new();
    private List<ItemType> itemTypes = new List<ItemType>(); //delete

    [SerializeField] private GameObject PlayerPrefab;
    [SerializeField] private GameObject CameraPrefab;
    [SerializeField] private GameObject DisableZonePrefab;

    private GameObject player;
    private GameObject playerCamera;
    private PlayerData playerData;
    private HUDUpdate playerHUDUpdate;
    private HUDWindow playerHUDUI;

    private SwipeController swipeController = new();
    private MovementController movementController = new();

    private ObjectsPool objectsPool = new();
    private Coroutine objectsSpawnRoutine;
    private Coroutine boostersSpawnRoutine;

    private int levelConfig = 0;
    private const float SPEED_MODIFY_MULTIPLIER = 0.075f;
    private const float SPAWN_MODIFY_MULTIPLIER = 0.025f;

    private void Awake()
    {
        var levelData = LevelsConfig.Instance.Levels[levelConfig];
        RoadGenerator.Initialize(levelData.RoadParts.Select(rp => AddressableExtensions.GetAsset(rp).GetComponent<RoadPart>()).ToList());
        DecorationGenerator.Initialize(levelData.DecorationParts.Select(rp => AddressableExtensions.GetAsset(rp).GetComponent<RoadPart>()).ToList());
        ObjectGenerator.Initialize(levelData.Obstacles.Select(o => AddressableExtensions.GetAsset(o)).ToList(),
            levelData.Collectables.Select(c => AddressableExtensions.GetAsset(c)).ToList());
        AddressablesAssetsHandler.ReleaseReferences();

        CreatePlayerWithData();
        CreateCamera();
        CreateDisableZone();
        playerData.GetCurrentGoals(levelData);

        swipeController.SetPlayerController(player.GetComponent<PlayerController>());
        movementController.SetInstance();

        RoadGenerator.SetContainer(CreateContainer("RoadPartsContainer", new(0, 0, -15)));
        ObjectGenerator.SetContainers(CreateContainer("ObstacleContainer"), CreateContainer("CollectableContainer", new(0, 1, 0)));
        ObjectGenerator.SetObjectPool(objectsPool);
        DecorationGenerator.SetContainer(CreateContainer("DecorationContainer", new(0, 0, -15)));
    }

    private void Start()
    {
        playerHUDUpdate = WindowsManager.Instance.GetHUDUpdate();
        PlayerDataUpdateSubscribes();
        RoadGenerator.CreateFullRoad(20);
        DecorationGenerator.CreateFullRoad(20);
        objectsSpawnRoutine = StartCoroutine(ObjectGenerator.SpawnObjects());
        boostersSpawnRoutine = StartCoroutine(ObjectGenerator.SpawnBoosters());

        itemController.ActivatePassiveItems();
    }

    private void OnEnable()
    {
        Time.timeScale = 1;
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
    private void CreatePlayerWithData()
    {
        player = Instantiate(PlayerPrefab);
        player.gameObject.name = "Player";
        playerData = player.GetComponent<PlayerData>();
    }

    private void CreateCamera()
    {
        playerCamera = Instantiate(CameraPrefab);
        playerCamera.name = "Camera";
        CinemachineFreeLook freeLookCamera = playerCamera.GetComponentInChildren<CinemachineFreeLook>();
        freeLookCamera.Follow = player.transform;
        freeLookCamera.LookAt = player.GetComponent<PlayerController>().CameraLookAt;
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
        playerData.UpdateStreak += ChangeSpeed;
        playerData.UpdateStreak += ChangeSpawnTime;
        playerData.GameOver += GameOver;

        //HUD updates
        playerData.UpdatePlayerLifes += playerHUDUpdate.UpdateLifes;
        playerData.UpdatePlayerScore += playerHUDUpdate.UpdateScore;
        playerData.UpdateStreak += playerHUDUpdate.UpdateStreak;
        playerData.UpdateFearEssence += playerHUDUpdate.UpdateFearEssence;
        playerData.UpdateBoosterIcon += playerHUDUpdate.UpdateBoosterIcon;
        playerData.UpdateLevelComplete += playerHUDUpdate.LevelDone;

        playerHUDUI = playerHUDUpdate.GetComponent<HUDWindow>();
        playerHUDUI.Click += UseItemByIndex;
        playerHUDUI.InitializeButtons(itemController.GetSlotTypes());
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

    public void ActivatePassiveItems()
    {
        itemController.ActivatePassiveItems();
    }

    public void UseItemByIndex(int index)
        => itemController.TryUseItem(index);

    public void GetInventorySlots(List<ItemType> items)
    {
        itemController.Initialize(items);
    }

    public void SetLevelConfig(int level)
    {
        levelConfig = level;
    }

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void GameOver()
    {
        WindowsManager.Instance.OpenPopup(WindowType.ClaimRewardPopup);
        Time.timeScale = 0;
    }
}
