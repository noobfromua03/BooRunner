﻿[System.Serializable]
public class TotemOfFearEssence : IInventoryItem
{
    public ItemType Type => ItemType.TotemOfFearEssence;
    public ItemSubType SubType => ItemSubType.passive;
    public IconType IconType => IconType.TotemOfFearEssence;
    public string Name => "Totem of fear essence";
    public bool ActionHandler()
    {
        PlayerData.Instance.TotemOfFearEssence();
        return true;
    }
}