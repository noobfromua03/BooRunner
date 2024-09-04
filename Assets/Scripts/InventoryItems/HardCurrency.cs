[System.Serializable]
public class HardCurrency : IInventoryItem
{
    public ItemType Type => ItemType.HardCurrency;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.HardCurrency; 
    public string Name => "Skulls";

    public void ActionHandler()
    {
    }
}