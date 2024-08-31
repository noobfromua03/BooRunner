[System.Serializable]
public class BottledEctoplasm : IInventoryItem
{
    public ItemType Type => ItemType.BottledEctoplasm;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.BottledEctoplasm;
    public void ActionHandler()
    {
        PlayerData.Instance.ImmaterialityBooster();
    }
}