[System.Serializable]
public class GemOfStorm : IInventoryItem
{
    public ItemType Type => ItemType.GemOfStorm;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.GemOfStorm;
    public void ActionHandler()
    {
        PlayerData.Instance.DarkCloudON();
    }
}