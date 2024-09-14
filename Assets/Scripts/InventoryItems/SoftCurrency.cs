[System.Serializable]
public class SoftCurrency : IInventoryItem
{
    public ItemType Type => ItemType.SoftCurrency;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.SoftCurrency;
    public string Name => "Cursed gold";
    public bool ActionHandler()
    {
        return true;
    }
}