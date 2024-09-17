
using System.Linq;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class MenuCompositionController : MonoBehaviour
{
    [SerializeField] private GameObject DisableZonePrefab;
    [SerializeField] private Transform disableZoneContainer;
    [SerializeField] private Transform enemyContainer;
    [SerializeField] private Transform ghost;
    [SerializeField] private LayerMask layerMask;

    private MovementController movementController = new();
    private ObjectsPool objectsPool = new();
    private ObjectGenerator generator = new();
    private Animator animator;
    private bool customHero;


    private Coroutine spawnRoutine;
    private void Awake()
    {
        var levelData = LevelsConfig.Instance.Levels[0];

        generator.InitializeForMenu(
            levelData.Obstacles.Select(o => AddressableExtensions.GetAsset(o)).ToList());

        movementController.SetInstance();
        generator.SetContainers(enemyContainer, null);
        generator.SetObjectPool(objectsPool);
        generator.SetSpawnDistance(19);

        animator = ghost.GetComponent<Animator>();

        Instantiate(DisableZonePrefab, disableZoneContainer);
    }

    private void Start()
    {
        spawnRoutine = StartCoroutine(generator.SpawnEnemiesForMenu());
    }

    private void Update()
    {
        if(EnemiesCheckRaycastUpdate(out var hit))
            UsingBooAnimation(hit);
            
        movementController.Update();
        movementController.FixedUpdate();
    }

    public void DisableSpawn(bool stopSpawn)
    {
        if (stopSpawn)
            StopCoroutine(spawnRoutine);
        else
            StartCoroutine(generator.SpawnEnemiesForMenu());

    }

    public PlayerCustomization GetCustomizator()
        => ghost.GetComponent<PlayerCustomization>();


    private bool EnemiesCheckRaycastUpdate(out RaycastHit hit)
     => Physics.Raycast(ghost.transform.position, Vector3.left, out hit, 10, layerMask);

    private void UsingBooAnimation(RaycastHit hit)
    {
        if (hit.collider.gameObject.TryGetComponent<Enemy>(out var enemy))
        {
            if (enemy.actionDone == false)
            {
                animator.SetTrigger("Boo");
                enemy.actionDone = true;
                enemy.ScaredJump();
            }
        }
    }

}

