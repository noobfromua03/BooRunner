[System.Serializable]
public class GemOfStorm : IInventoryItem
{
    public ItemType Type => ItemType.GemOfStorm;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.GemOfStorm;
    public string Name => "Gem of Storm";

    public bool ActionHandler()
    {
        PlayerData.Instance.DarkCloudON();
        return true;
    }
}