[System.Serializable]
public class CoffinKey : IInventoryItem
{
    public ItemType Type => ItemType.CoffinKey;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.CoffinKey;
    public void ActionHandler()
    {
        PlayerData.Instance.CoffinKeyON();
    }
}