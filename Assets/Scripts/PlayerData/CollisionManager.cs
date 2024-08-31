using System.Collections.Generic;
using System.Linq;
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

        poolObjects?.FindAll(o => o.ActionDone() == false).ForEach(o => o.ActionHandler());

        if (PlayerData.Instance.IsPhantomOfTheOpera.Status)
        {
            var enemies = PhantomOfTheOperaCollisionCheck();

            enemies?.FindAll(o => o.ActionDone() == false).ForEach(o => o.PhantomOfTheOperaHandler());
        }

    }

    private List<IPoolObject> PlayerCollisionCheck()
    {
        var ColliderHits = Physics.OverlapSphere(playerCollisionPoint.position, OVERLAP_RADIUS, objectsMask);

        return ColliderHits.Length > 0 ? ColliderHits.Select(po => po.GetComponent<IPoolObject>()).ToList() : null;
    }

    private List<Enemy> PhantomOfTheOperaCollisionCheck()
    {
        var ColliderHits = Physics.OverlapSphere(playerCollisionPoint.position, PHANTOM_OF_THE_OPERA_OVERLAP_RADIUS, objectsMask);

        return ColliderHits.Length > 0 ? ColliderHits.Select(e => e.GetComponent<Enemy>()).Where(e => e != null).ToList() : null;
    }

}
