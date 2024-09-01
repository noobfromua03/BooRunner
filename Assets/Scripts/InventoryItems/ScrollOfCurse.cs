[System.Serializable]
public class ScrollOfCurse : IInventoryItem
{
    public ItemType Type => ItemType.ScrollOfCurse;
    public ItemSubType SubType => ItemSubType.active; 
    public IconType IconType => IconType.ScrollOfCurse;
    public void ActionHandler()
    {
        PlayerData.Instance.ScrollOfCurse();
    }
}