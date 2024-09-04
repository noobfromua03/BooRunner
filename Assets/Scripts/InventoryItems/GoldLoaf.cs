[System.Serializable]
public class GoldLoaf : IInventoryItem
{
    public ItemType Type => ItemType.GoldLoaf;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.GoldLoaf;
    public string Name => "Gold loaf";
    public void ActionHandler()
    {
        PlayerData.Instance.GoldBreadON();
    }
}