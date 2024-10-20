using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ObjectGenerator
{
    private List<GameObject> obstaclePrefabs;
    private List<GameObject> collectablePrefabs;
    private Transform obstaclesContainer;
    private Transform collectableContainer;

    private List<SpawnVariant> spawnVariants = new()
    {
        new SpawnVariant(true, false, false),
        new SpawnVariant(true, false, false),
        new SpawnVariant(true, false, false),
        new SpawnVariant(true, true, false),
        new SpawnVariant(true, true, false),
        new SpawnVariant(true, false, true),
        new SpawnVariant(true, false, true),
        new SpawnVariant(false, true, false),
        new SpawnVariant(false, true, false),
        new SpawnVariant(false, true, false),
        new SpawnVariant(false, false, true),
        new SpawnVariant(false, false, true),
        new SpawnVariant(false, false, true),
        new SpawnVariant(false, true, true),
        new SpawnVariant(false, true, true),
        new SpawnVariant(true, true, true)
    };

    private List<SpawnVariant> menuSpawnVariants = new()
    {
        new SpawnVariant(true, false, false),
        new SpawnVariant(false, true, false),
        new SpawnVariant(false, false, true)
    };

    private ObjectsPool objectsPool;

    private float objectsSpawnTime = 0.5f;
    private float boostersSpawnTime = 15f;
    private int essenceVariantCounter = 0;
    private SpawnVariant lastEssenceVariant;
    private float spawnDistance = 30;

    private int RandomLine => GetRandomValue(0, 3);


    public void Initialize(List<GameObject> obstaclePrefabs, List<GameObject> collectablePrefabs)
    {
        this.obstaclePrefabs = obstaclePrefabs;
        this.collectablePrefabs = collectablePrefabs;
    }

    public void InitializeForMenu(List<GameObject> enemyPrefabs)
    {
        obstaclePrefabs = enemyPrefabs.Where(p => p.GetComponent<Enemy>()).ToList();
    }

    public void GenerateNewObjectOnLine(int line, Transform container, PoolObjectType type, List<GameObject> prefabs)
    {
        var poolObject = prefabs.Find(po => po.GetComponent<IPoolObject>().ObjectType == type).GetComponent<IPoolObject>();
        poolObject = objectsPool.Spawn(poolObject, container, GetPositionObject(container));
        poolObject.ChangeLine(line);
    }

    public void GenerateAllPossibleObjectsOnLevel()
    {
        for (int i = 0; i < obstaclePrefabs.Count; i++)
        {
            var prefab = obstaclePrefabs[i].GetComponent<IPoolObject>();
            var poolObject = objectsPool.Spawn(prefab, obstaclesContainer, GetPositionObject(obstaclesContainer)) as MonoBehaviour;
            poolObject.gameObject.SetActive(false);
        }

    }

    private Vector3 GetPositionObject(Transform container)
    {
        return new Vector3(0, container.position.y, spawnDistance);
    }

    private T GetRandomEnumValue<T>() where T : Enum
        => (T)Enum.GetValues(typeof(T)).GetValue(GetRandomValue(0, Enum.GetValues(typeof(T)).Length));

    private T GetEnumValueByIndex<T>(int i) where T : Enum
        => (T)Enum.ToObject(typeof(T), i);

    private int GetRandomValue(int min, int max)
        => UnityEngine.Random.Range(min, max);

    public void AutomaticSpawnObstaclesByVariants()
    {
        var currentVariant = spawnVariants[GetRandomValue(0, spawnVariants.Count)];
        List<bool> lines = new List<bool>() { currentVariant.Left, currentVariant.Middle, currentVariant.Right };

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i])
            {
                var type = obstaclePrefabs.OrderBy(o => UnityEngine.Random.value).First()
                    .GetComponent<IPoolObject>().ObjectType;
                var poolObject = objectsPool.GetObjectFromPoolByType(type);

                if (poolObject != null)
                {
                    switch (poolObject.GetComponent<IPoolObject>())
                    {
                        case Obstacle:
                            poolObject.GetComponent<Obstacle>().ChangeLine(i);
                            poolObject.GetComponent<Obstacle>().Rotate();
                            break;
                        case Enemy:
                            poolObject.GetComponent<Enemy>().ChangeLine(i);
                            poolObject.GetComponent<Enemy>().GetPatrolLine(lines, i);
                            break;
                    }

                    poolObject.transform.position = GetPositionObject(obstaclesContainer);
                    poolObject.SetActive(true);
                }
                else
                    GenerateNewObjectOnLine(i, obstaclesContainer, type, obstaclePrefabs);
            }
        }

        essenceVariantCounter = -1;
    }

    public void AutomaticSpawnEnemiesForMenu()
    {
        var currentVariant = menuSpawnVariants[GetRandomValue(0, menuSpawnVariants.Count)];
        List<bool> lines = new List<bool>() { currentVariant.Left, currentVariant.Middle, currentVariant.Right };

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i])
            {
                var type = obstaclePrefabs.OrderBy(o => UnityEngine.Random.value).First()
                    .GetComponent<IPoolObject>().ObjectType;
                var poolObject = objectsPool.GetObjectFromPoolByType(type);

                if (poolObject != null)
                {
                    poolObject.GetComponent<Enemy>().ChangeLine(i);
                    poolObject.transform.position = GetPositionObject(obstaclesContainer);
                    poolObject.SetActive(true);
                }
                else
                    GenerateNewObjectOnLine(i, obstaclesContainer, type, obstaclePrefabs);
            }
        }
    }

    public void AutomaticSpawnCollectableObjectsByVariants()
    {
        var currentVariant = spawnVariants[GetRandomValue(0, spawnVariants.Count)];

        if (essenceVariantCounter == 0)
            lastEssenceVariant = currentVariant;
        else
            currentVariant = lastEssenceVariant;

        List<bool> lines = new List<bool>() { currentVariant.Left, currentVariant.Middle, currentVariant.Right };

        for (int i = 0; i < lines.Count; i++)
        {
            if (lines[i])
            {
                var type = PoolObjectType.FearEssence;

                if (collectablePrefabs.All(c => c.GetComponent<IPoolObject>().ObjectType != type))
                {
                    Debug.LogError("FearEssence doesnt exist in prefabs list");
                    continue;
                }

                var poolObject = objectsPool.GetObjectFromPoolByType(type);

                if (poolObject != null)
                {
                    poolObject.transform.position = GetPositionObject(collectableContainer);
                    poolObject.GetComponent<FearEssence>().ChangeLine(i);
                    poolObject.SetActive(true);
                }
                else
                    GenerateNewObjectOnLine(i, collectableContainer, type, collectablePrefabs);
            }
        }
    }

    public void AutomaticSpawnBoosters()
    {
        var type = collectablePrefabs.OrderBy(c => UnityEngine.Random.value).
            Where(c => c.GetComponent<IPoolObject>().ObjectType != PoolObjectType.FearEssence).First()
            .GetComponent<IPoolObject>().ObjectType;

        var poolObject = objectsPool.GetObjectFromPoolByType(type);

        if (poolObject != null)
        {
            poolObject.GetComponent<IPoolObject>().ChangeLine(RandomLine);
            poolObject.transform.position = GetPositionObject(collectableContainer);
            poolObject.SetActive(true);
        }
        else
            GenerateNewObjectOnLine(RandomLine, collectableContainer, type, collectablePrefabs);
    }

    public IEnumerator SpawnObjects()
    {
        while (true)
        {
            yield return new WaitForSeconds(objectsSpawnTime);
            if (essenceVariantCounter == 3)
                AutomaticSpawnObstaclesByVariants();
            else
                AutomaticSpawnCollectableObjectsByVariants();
            essenceVariantCounter++;

            spawnDistance = Mathf.Clamp(spawnDistance += 1, 30, 100);
        }
    }

    public IEnumerator SpawnEnemiesForMenu()
    {
        while (true)
        {
            yield return new WaitForSeconds(GetRandomValue(1, 10));
            AutomaticSpawnEnemiesForMenu();
        }
    }

    public IEnumerator SpawnBoosters()
    {
        while (true)
        {
            yield return new WaitForSeconds(boostersSpawnTime);
            AutomaticSpawnBoosters();
        }
    }

    public void SetObjectPool(ObjectsPool pool)
    {
        objectsPool = pool;
    }

    public void SetContainers(Transform obstacles, Transform collectable)
    {
        obstaclesContainer = obstacles;
        collectableContainer = collectable;
    }

    public void ChangeSpawnTime(float value)
    {
        objectsSpawnTime = Mathf.Clamp(0.5f - value, 0.2f, 0.5f);
    }

    public void SetSpawnDistance(float value)
        => spawnDistance = value;
}


