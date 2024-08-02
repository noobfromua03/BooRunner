using System.Collections.Generic;
using UnityEngine;

public class CollisionManager : MonoBehaviour
{
    [SerializeField] private Transform playerCollisionPoint;
    [SerializeField] private LayerMask objectsMask;

    private const float OVERLAP_RADIUS = 0.55f;
    private const float PHANTOM_OF_THE_OPERA_OVERLAP_RADIUS = 3f;
    private void Update()
    {
        PlayerCollisionEnter();
    }

    public void PlayerCollisionEnter()
    {
        var poolObjects = PlayerCollisionCheck();

        if (poolObjects != null)
        {
            foreach (var obj in poolObjects)
            {
                if (obj.ActionDone() == false)
                    obj.ActionHandler();
            }
        }

        if (PlayerData.instance.IsPhantomOfTheOpera)
        {
            var enemies = PhantomOfTheOperaCollisionCheck();

            if (enemies != null)
            {
                foreach (var enemy in enemies)
                {
                    if (enemy.ActionDone() == false)
                        enemy.ActionHandler();
                }
            }
        }

    }

    private List<IPoolObject> PlayerCollisionCheck()
    {
        var ColliderHits = Physics.OverlapSphere(playerCollisionPoint.position, OVERLAP_RADIUS, objectsMask);
        List<IPoolObject> poolObjects = new();

        if (ColliderHits.Length > 0)
        {
            foreach (var poolObject in ColliderHits)
            {
                poolObjects.Add(poolObject.GetComponent<IPoolObject>());
            }
            return poolObjects;
        }
        return null;
    }

    private List<Enemy> PhantomOfTheOperaCollisionCheck()
    {
        var ColliderHits = Physics.OverlapSphere(playerCollisionPoint.position, PHANTOM_OF_THE_OPERA_OVERLAP_RADIUS, objectsMask);
        List<Enemy> enemies = new();
        if (ColliderHits.Length > 0)
        {
            foreach (var enemyCollider in ColliderHits)
            {
                if (enemyCollider.gameObject.TryGetComponent<Enemy>(out var enemy))
                    enemies.Add(enemy);
            }
            return enemies;
        }
        return null;
    }

}
