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
    ChillingTouch = 8,
    PhantomOfTheOpera = 9,
    TownLegend = 10,
    BottledEctoplasm = 11,
    BrokenClock = 12,
    GemOfStorm = 13,
    DarkRune = 14,
    AmuletOfFearAura = 15,
    MysticBook = 16,
    ScrollOfCurse = 17,
    HeartOfGhost = 18,
    ScareTotem = 19,
    GoldLoaf = 20,
    CoffinKey = 21,
    TotemOfFearEssence = 22,
    None = 23,
    RealCurrency = 24,
    CowboyHat = 25,
    CrownHat = 26,
    MagicalHat = 27,
    MinerHat = 28,
    PajamaHat = 29,
    PillboxHat = 30,
    PoliceCapHat = 31,
    ShowerCapHat = 32,
    SombreroHat = 33,
    VikingHelmetHat = 34

}
