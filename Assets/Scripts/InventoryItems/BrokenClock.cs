[System.Serializable]
public class BrokenClock : IInventoryItem
{
    public ItemType Type => ItemType.BrokenClock;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType =>IconType.BrokenClock;
    public string Name => "Broken Clock";
    public void ActionHandler()
    {
        PlayerData.Instance.SlowMotionsON();
    }
}