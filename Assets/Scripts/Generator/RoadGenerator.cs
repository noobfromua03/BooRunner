using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RoadGenerator
{
    private List<RoadPart> roadPartsPrefabs;
    
    private List<RoadPart> road = new();
    private RoadPart lastRoadPart;
    private Transform container;

    public void Initialize(List<RoadPart> roadParts)
    {
        roadPartsPrefabs = roadParts;
    }

    public void CreateFullRoad(int leng)
    {
        for (int i = 0; i < leng; i++)
        {
            var prefab = GetRandomRoadPartPrefab();
            var roadPart = Object.Instantiate(prefab, container);
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

    public void SetContainer(Transform container)
    {
        this.container = container;
    }
}

