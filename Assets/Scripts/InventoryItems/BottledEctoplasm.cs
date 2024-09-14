[System.Serializable]
public class BottledEctoplasm : IInventoryItem
{
    public ItemType Type => ItemType.BottledEctoplasm;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.BottledEctoplasm;
    public string Name => "Bottled Ectoplasm"; 
    public bool ActionHandler()
    {
        PlayerData.Instance.ImmaterialityBooster();
        return true;
    }
}