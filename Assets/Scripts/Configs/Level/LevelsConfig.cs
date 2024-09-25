using System;
using System.Collections.Generic;
using UnityEditor.Searcher;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelsConfig : AbstractConfig<LevelsConfig>
{
    [field: SerializeField] public List<LevelData> Levels { get; private set; }
    [field: SerializeField] public List<EnvironmentData> EnvironmentData { get; private set; }
    [field: SerializeField] public List<Material> SkyBoxes { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> Collectables { get; private set; }

    public void OnValidate()
    {
        for(int i = 0; i <  Levels.Count; i++)
        {
            foreach (var item in Levels[i].Goals)
                item.GoalTextBuilder(item.Type);
        }
    }

    public Material GetSkyBoxByType(SkyBoxType type)
    {
        switch(type)
        {
            case SkyBoxType.Day:
                return SkyBoxes[0];
            case SkyBoxType.Night:
                return SkyBoxes[1];
        }
        return null;
    }

}

[System.Serializable]
public class LevelData
{
    [field: SerializeField] public EnvironmentType EnvironmentType;
    [field: SerializeField] public List<GoalsData> Goals { get; private set; }
    [field: SerializeField] public float LightTemperature { get; private set; }
    public EnvironmentData GetEnvironmentByType(EnvironmentType type)
        => LevelsConfig.Instance.EnvironmentData.Find(e => e.Type == type);


}

[Serializable]
public class EnvironmentData
{
    [field: SerializeField] public EnvironmentType Type { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> Obstacles { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> RoadParts { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> DecorationParts { get; private set; }
    [field: SerializeField] public AudioType AudioType { get; private set; }
    [field: SerializeField] public SkyBoxType SkyBox { get; private set; }
}

public enum EnvironmentType
{
    CityPark = 0,
    Farm = 1,
    Castle = 2,
    Town = 3,
    Cemetry = 4
}

public enum SkyBoxType
{
    Day = 0,
    Night = 1
}