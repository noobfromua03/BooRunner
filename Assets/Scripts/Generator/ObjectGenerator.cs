using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGenerator : MonoBehaviour
{
    [SerializeField] private List<GameObject> obstaclePrefabs = new List<GameObject>();
    [SerializeField] private Transform container;
    [SerializeField] private ObjectsPool objectsPool;

    private List<SpawnVariant> spawnVariants = new()
    {
        new SpawnVariant(true, false, false),
        new SpawnVariant(true, true, false),
        new SpawnVariant(true, false, true),
        new SpawnVariant(false, true, false),
        new SpawnVariant(false, false, true),
        new SpawnVariant(false, true, true),
        new SpawnVariant(true, true, true)
    };

    private float spawnTime = 1.5f;
    private Coroutine spawnRoutine;

    private void Start()
    {
        spawnRoutine = StartCoroutine(spawnObstacles());
    }
    private void Update()
    {
        InstantiateObstacleByKey();
    }

    public void GenerateNewObjectOnLine(int line)
    {
        if (obstaclePrefabs.Count > 0)
        {
            GameObject newObstaclePrefab = obstaclePrefabs[GetRandomValue(obstaclePrefabs.Count)];
            var poolObject = objectsPool.Spawn(newObstaclePrefab.GetComponent<IPoolObject>(), container, GetPositionObject());
            poolObject.ChangeLine(line);
        }
    }

    public Vector3 GetPositionObject()
    {
        return Vector3.forward * 40;
    }

    public void InstantiateObstacleByKey()
    {
        bool keyDown = Input.GetButtonDown("Up");
        if (keyDown)
        {
            var poolObject = objectsPool.GetObjectFromPoolByType(GetRandomEnumValue<PoolObjectType>());

            if (poolObject != null)
            {
                poolObject.transform.position = GetPositionObject();
                poolObject.GetComponent<Obstacle>().MoveUnit.PointNumber = GetRandomValue(3);
                poolObject.SetActive(true);
            }
            else
                GenerateNewObjectOnLine(GetRandomValue(3));
        }
    }

    private T GetRandomEnumValue<T>() where T : Enum
        => (T)Enum.GetValues(typeof(T)).GetValue(GetRandomValue(Enum.GetValues(typeof(T)).Length));

    private int GetRandomValue(int max)
        => UnityEngine.Random.Range(0, max);

    private void AutomaticSpawnObjectsByVariants()
    {
        var currentVariant = spawnVariants[GetRandomValue(spawnVariants.Count)];
        List<bool> lines = new List<bool>() { currentVariant.Left, currentVariant.Middle, currentVariant.Right };


        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i])
            {
                var poolObject = objectsPool.GetObjectFromPoolByType(GetRandomEnumValue<PoolObjectType>());

                if (poolObject != null)
                {
                    switch (poolObject.GetComponent<IPoolObject>())
                    {
                        case Obstacle:
                            poolObject.GetComponent<Obstacle>().ChangeLine(i);
                            break;
                        case Enemy:
                            poolObject.GetComponent<Enemy>().ChangeLine(i);
                            poolObject.GetComponent<Enemy>().GetPatrolLine(lines, i);
                            break;
                    }

                    poolObject.transform.position = GetPositionObject();
                    poolObject.SetActive(true);
                }
                else
                {
                    GenerateNewObjectOnLine(i);
                }
            }
        }
    }


    private IEnumerator spawnObstacles()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnTime);
            AutomaticSpawnObjectsByVariants();
        }
    }
}


