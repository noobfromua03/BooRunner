[System.Serializable]
public class OldKey : IInventoryItem
{
    public ItemType Type => ItemType.OldKey;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.OldKey;
    public string Name => "Old Key";

    public bool ActionHandler()
    {
        PlayerData.Instance.OldKeyON();
        return true;
    }
}