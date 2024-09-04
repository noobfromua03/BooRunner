[System.Serializable]
public class NoneItem : IInventoryItem
{
    public ItemType Type => ItemType.None;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.None;
    public string Name => "None";
    public void ActionHandler()
    {
    }
}