[System.Serializable]
public class MysticBook : IInventoryItem
{
    public ItemType Type => ItemType.MysticBook;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.MysticBook;
    public string Name => "Mystic book";
    public bool ActionHandler()
    {
        PlayerData.Instance.TownLegendON();
        return true;
    }
}