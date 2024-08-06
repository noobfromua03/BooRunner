using System.Collections.Generic;
using UnityEngine;

public class LevelsConfig : AbstractConfig<LevelsConfig>
{
    [field: SerializeField] public List<LevelData> Levels { get; private set; }

}

[System.Serializable]
public class LevelData
{ 
    [field: SerializeField] public List<GameObject> Obstacles { get; private set; }
    [field: SerializeField] public List<GameObject> Collectables { get; private set; }
    [field: SerializeField] public List<RoadPart> RoadParts { get; private set; }
    [field: SerializeField] public List<RoadPart> DecorationParts { get; private set; }
}