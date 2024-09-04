[System.Serializable]
public class HeartOfGhost : IInventoryItem
{
    public ItemType Type => ItemType.HeartOfGhost;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.HeartOfGhost;
    public string Name => "Heart of ghost";
    public void ActionHandler()
    {
        PlayerData.Instance.AddLife(1);
    }
}