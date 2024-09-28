using System.Linq;
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

    private ItemController itemController = new();
    private GameObject player;
    private GameObject playerCamera;
    private PlayerData playerData;
    private HUD playerHUDUpdate;
    private HUDWindow playerHUDUI;
    private SwipeController swipeController = new();
    private MovementController movementController = new();
    private ObjectsPool objectsPool = new();
    private float lightTemperature;
    private AudioType levelMusic;

    private int levelConfig = 0;
    private const float SPEED_MODIFY_MULTIPLIER = 0.075f;
    private const float SPAWN_MODIFY_MULTIPLIER = 0.025f;

    private void Awake()
    {
        var levelData = LevelsConfig.Instance.Levels[levelConfig];
        var environmentData = levelData.GetEnvironmentByType(levelData.EnvironmentType);

        RoadGenerator.Initialize(
            environmentData
                .RoadParts.Select(rp => AddressableExtensions.GetAsset(rp).GetComponent<RoadPart>())
                .ToList());
        DecorationGenerator.Initialize(
            environmentData
                .DecorationParts.Select(rp =>
                    AddressableExtensions.GetAsset(rp).GetComponent<RoadPart>()
                ).ToList());
        ObjectGenerator.Initialize(
            environmentData.Obstacles.Select(o => AddressableExtensions.GetAsset(o)).ToList(),
            LevelsConfig.Instance.Collectables.Select(c => AddressableExtensions.GetAsset(c)).ToList());
        AddressablesAssetsHandler.ReleaseReferences();

        CreatePlayerWithData();
        CreateCamera();
        RenderSettings.skybox = LevelsConfig.Instance.GetSkyBoxByType(environmentData.SkyBox);
        levelMusic = environmentData.AudioType;
        lightTemperature = levelData.LightTemperature;
        CreateDisableZone();
        playerData.GetCurrentGoals(levelData, levelConfig);

        swipeController.SetPlayerController(player.GetComponent<PlayerController>());
        movementController.SetInstance();

        RoadGenerator.SetContainer(CreateContainer("RoadPartsContainer", new(0, 0, -15)));
        ObjectGenerator.SetContainers(
            CreateContainer("ObstacleContainer"),
            CreateContainer("CollectableContainer", new(0, 1, 0))
        );
        ObjectGenerator.SetObjectPool(objectsPool);
        DecorationGenerator.SetContainer(CreateContainer("DecorationContainer", new(0, 0, -15)));
    }

    private void Start()
    {
        itemController.Initialize();

        playerHUDUpdate = WindowsManager.Instance.GetHUDUpdate();
        InitializePlayerEvents();

        ObjectGenerator.GenerateAllPossibleObjectsOnLevel();
        RoadGenerator.CreateFullRoad(20);
        DecorationGenerator.CreateFullRoad(20);
        StartCoroutine(ObjectGenerator.SpawnObjects());
        StartCoroutine(ObjectGenerator.SpawnBoosters());

        playerCamera.GetComponentInChildren<Light>().colorTemperature = lightTemperature;

        itemController.ActivatePassiveItems();
        AudioManager.Instance.PlayAudioByType(levelMusic, AudioSubType.Music);
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
        player = Instantiate(PlayerPrefab).With(p => p.gameObject.name = "Player");
        playerData = player.GetComponent<PlayerData>();
    }

    private void CreateCamera()
    {
        playerCamera = Instantiate(CameraPrefab).With(p => p.name = "Camera");

        CinemachineFreeLook freeLookCamera =
            playerCamera.GetComponentInChildren<CinemachineFreeLook>();
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

    private void InitializePlayerEvents()
    {
        playerData.UpdateStreak += ChangeSpeed;
        playerData.UpdateStreak += ChangeSpawnTime;
        playerData.GameOver += GameOver;

        playerData.UpdatePlayerLifes += playerHUDUpdate.UpdateLifes;
        playerData.UpdatePlayerScore += playerHUDUpdate.UpdateScore;
        playerData.UpdateStreak += playerHUDUpdate.UpdateStreak;
        playerData.UpdateBoosterIcon += playerHUDUpdate.UpdateBoosterIcon;
        playerData.UpdateLevelComplete += playerHUDUpdate.LevelDone;

        playerHUDUI = playerHUDUpdate.GetComponent<HUDWindow>();
        itemController.OnItemUsed += playerHUDUI.UpdateSlotsAmount;
        playerHUDUI.Click += UseItemByIndex;
        playerHUDUI.InitializeButtons(itemController.GetSlotTypes());
    }

    private void ChangeSpeed(int streak)
        => movementController.ChangeSpeedModify(GetSlowMotionMultiplier(streak, playerData.IsSlowMotion.Status, SPEED_MODIFY_MULTIPLIER));

    private void ChangeSpawnTime(int streak)
        => ObjectGenerator.ChangeSpawnTime(GetSlowMotionMultiplier(streak, playerData.IsSlowMotion.Status, SPAWN_MODIFY_MULTIPLIER));

    private float GetSlowMotionMultiplier(int streak, bool status, float multiplier)
        => status ? streak * multiplier / 2 : streak * multiplier;

    public bool UseItemByIndex(int index)
        => itemController.TryUseItem(index);

    public void SetLevelConfig(int level)
        => levelConfig = level;

    public void Reload()
        => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    private void GameOver()
    {
        Time.timeScale = 0;

        AdvertWrapper.Instance.ShowInterstitial(null, (v) =>
        {
            ShowRewardPopup();
        }, () =>
        {
            ShowRewardPopup();
        });
    }

    private void ShowRewardPopup()
    {
        var rewardData = RewardConfig.Instance.GetSoftLevelReward(playerData.IsGoldLoaf, playerData.Score);
        var rewardPopup = WindowsManager.Instance.OpenPopup(WindowType.ClaimRewardPopup) as ClaimRewardPopup;

        CurrencyService.AddCurrency(CurrencyType.Soft, rewardData.Amount);
        rewardPopup.InitializeReward(rewardData);
    }
}
