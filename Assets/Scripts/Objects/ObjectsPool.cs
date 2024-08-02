using System.Collections.Generic;
using UnityEngine;

public class ObjectsPool
{
    private Dictionary<PoolObjectType, List<IPoolObject>> pools = new ();

    public void AddToPool(IPoolObject poolObject)
    {
        if (pools.ContainsKey(poolObject.ObjectType) == false)
        {
            pools.Add(poolObject.ObjectType, new List<IPoolObject>());
        }

        pools[poolObject.ObjectType].Add(poolObject);
    }

    public IPoolObject Spawn(IPoolObject prefab, Transform container, Vector3 position)
    {
        var gameObject = Object.Instantiate(prefab as MonoBehaviour, container);
        gameObject.transform.position = position;
        var poolObject = gameObject as IPoolObject;
        AddToPool(poolObject);
        return poolObject;
    }

    public GameObject GetObjectFromPoolByType(PoolObjectType type)
    {
        if (pools.ContainsKey(type) == false)
            return null;

        var pool = pools[type];

        foreach (var item in pool)
        {
            var monoBehaviourItem = item as MonoBehaviour;
            if (monoBehaviourItem != null && monoBehaviourItem.gameObject.activeSelf == false)
                return monoBehaviourItem.gameObject;
        }
        return null;
    }
}
