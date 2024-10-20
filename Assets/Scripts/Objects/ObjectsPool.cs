using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectsPool
{
    private Dictionary<PoolObjectType, List<IPoolObject>> pools = new ();

    public void AddToPool(IPoolObject poolObject)
    {
        if (pools.ContainsKey(poolObject.ObjectType) == false)
            pools.Add(poolObject.ObjectType, new List<IPoolObject>());

        pools[poolObject.ObjectType].Add(poolObject);
    }

    public IPoolObject Spawn(IPoolObject prefab, Transform container, Vector3 position)
    {
        var gameObject = GameObject.Instantiate(prefab as MonoBehaviour, container);
        gameObject.transform.position = position;
        var poolObject = gameObject as IPoolObject;
        AddToPool(poolObject);
        return poolObject;
    }

    public GameObject GetObjectFromPoolByType(PoolObjectType type)
    {
        if (pools.ContainsKey(type) == false)
            return null;

        var pool = pools[type].FindAll(o => o.ActiveStatus() == false);
        var poolItem = pool.Count > 0 ? pool.OrderBy(po => Random.value).First() as MonoBehaviour : null;
        
        return poolItem?.gameObject;
    }
}
