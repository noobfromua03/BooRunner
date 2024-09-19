using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class LevelsConfig : AbstractConfig<LevelsConfig>
{
    [field: SerializeField] public List<LevelData> Levels { get; private set; }



    public void OnValidate()
    {
        for(int i = 0; i <  Levels.Count; i++)
        {
            foreach (var item in Levels[i].Goals)
                item.GoalTextBuilder(item.Type);
        }
        
    }

}

[System.Serializable]
public class LevelData
{ 
    [field: SerializeField] public List<AssetReferenceGameObject> Obstacles { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> Collectables { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> RoadParts { get; private set; }
    [field: SerializeField] public List<AssetReferenceGameObject> DecorationParts { get; private set; }
    [field: SerializeField] public List<GoalsData> Goals { get; private set; }
    [field: SerializeField] public float LightTemperature { get; private set; }
}