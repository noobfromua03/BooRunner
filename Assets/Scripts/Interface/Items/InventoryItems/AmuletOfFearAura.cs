[System.Serializable]
public class AmuletOfFearAura : IInventoryItem
{
    public ItemType Type => ItemType.AmuletOfFearAura;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.AmuletOfFearAura;

    public void ActionHandler()
    {
        PlayerData.Instance.PhantomOfTheOperaON();
    }
}