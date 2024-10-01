[System.Serializable]
public class ScrollOfCurse : IInventoryItem
{
    public ItemType Type => ItemType.ScrollOfCurse;
    public ItemSubType SubType => ItemSubType.active; 
    public IconType IconType => IconType.ScrollOfCurse;
    public string Name => "Scroll of curse";
    public bool ActionHandler()
    {
        PlayerData.Instance.ChillingTouchON();
        return true;
    }
}