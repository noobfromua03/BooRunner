using UnityEngine;

[System.Serializable]
public class IconData
{
    [field: SerializeField] public IconType Type { get; private set; }
    [field: SerializeField] public Sprite Sprite { get; private set; }
}

public enum IconType
{
    Lifes = 0,
    ScaredStreak = 1,
    FearEssence = 2,
    SoftCurrency = 3,
    HardCurrency = 4,
    Immateriality = 5,
    SlowMotion = 6,
    DarkCloud = 7,
    LightsOff = 8,
    PhantomOfTheOpera = 9,
    TownLegend = 10
}
