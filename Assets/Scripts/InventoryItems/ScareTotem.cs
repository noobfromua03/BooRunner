[System.Serializable]
public class ScareTotem : IInventoryItem
{
    public ItemType Type => ItemType.ScareTotem;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.ScareTotem;
    public string Name => "Scare totem";
    public void ActionHandler()
    {
        PlayerData.Instance.ScareTotemON();
    }
}