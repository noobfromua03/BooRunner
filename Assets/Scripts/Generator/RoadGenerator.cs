using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour
{
    [SerializeField] private List<RoadPart> roadPartsPrefabs;
    [SerializeField] private Transform container;

    private List<RoadPart> road = new();
    private RoadPart lastRoadPart;

    private void Awake()
    {
        CreateFullRoad(50);
    }

    public void CreateFullRoad(int leng)
    {
        for (int i = 0; i < leng; i++)
        {
            var prefab = GetRandomRoadPartPrefab();
            var roadPart = Instantiate(prefab, container);
            roadPart.transform.position = (lastRoadPart == null) ? container.position : lastRoadPart.FinishPoint.position;
            lastRoadPart = roadPart;
            roadPart.onFinish += MoveOnTop;
            road.Add(roadPart);
        }
    }

    private RoadPart GetRandomRoadPartPrefab()
    {
        return roadPartsPrefabs[Random.Range(0, roadPartsPrefabs.Count)];
    }

    public void MoveOnTop(RoadPart roadPart)
    {
        roadPart.transform.position = lastRoadPart.FinishPoint.position;
        lastRoadPart = roadPart;
    }
}

