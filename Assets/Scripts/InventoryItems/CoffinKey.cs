[System.Serializable]
public class CoffinKey : IInventoryItem
{
    public ItemType Type => ItemType.CoffinKey;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.CoffinKey;
    public string Name => "Coffin Key";

    public bool ActionHandler()
    {
        PlayerData.Instance.CoffinKeyON();
        return true;
    }
}