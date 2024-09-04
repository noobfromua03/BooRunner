[System.Serializable]
public class DarkRune : IInventoryItem
{
    public ItemType Type => ItemType.DarkRune;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.DarkRune;
    public string Name => "Dark rune";

    public void ActionHandler()
    {
        PlayerData.Instance.LightsOFF();
    }
}