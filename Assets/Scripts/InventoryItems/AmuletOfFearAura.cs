[System.Serializable]
public class AmuletOfFearAura : IInventoryItem
{
    public ItemType Type => ItemType.AmuletOfFearAura;
    public ItemSubType SubType => ItemSubType.active;
    public IconType IconType => IconType.AmuletOfFearAura;
    public string Name => "Amulet of fear aura";

    public void ActionHandler()
    {
        PlayerData.Instance.PhantomOfTheOperaON();
    }
}