public interface IInventoryItem
{
    public ItemType Type { get; }
    public ItemSubType SubType { get; }
    public IconType IconType { get; }
    public string Name { get; }
    public void ActionHandler();
}

public enum ItemType
{
    BottledEctoplasm = 0,
    BrokenClock = 1,
    GemOfStorm = 2,
    DarkRune = 3,
    AmuletOfFearAura = 4,
    MysticBook = 5,
    ScrollOfCurse = 6,
    HeartOfGhost = 7,
    ScareTotem = 8,
    GoldLoaf = 9,
    CoffinKey = 10,
    TotemOfFearEssence = 11,
    None = 12,
    SoftCurrency = 13,
    HardCurrency = 14
}

public enum ItemSubType
{
    active = 0,
    passive = 1
}

public enum CurrencyType
{
    Soft = 0,
    Hard = 1,
    Real = 2
}