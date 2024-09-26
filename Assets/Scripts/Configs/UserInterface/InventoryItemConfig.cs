using System.Collections.Generic;
using UnityEngine;
/*
public class InventoryItemConfig : AbstractConfig<InventoryItemConfig>
{
    [field: SerializeField] public List<ItemData> ItemData { get; private set; }

    public void OnValidate()
    {
        foreach (var item in ItemData)
            item.OnValidate();
    }

    public IInventoryItem GetItemByType(ItemType type)
        => ItemData.Find(i => i.Type == type).Item;
}

[System.Serializable]
public class ItemData
{
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeReference] public IInventoryItem Item { get; private set; }

    public void OnValidate()
    {
        if (Item == null || Item.Type != Type)
            Item = ItemBuilder.GetInventoryItem(Type);
    }
}*/
public static class ItemBuilder
{
    public static IInventoryItem GetInventoryItem(ItemType type)
        => type switch
        {
            ItemType.BottledEctoplasm => new BottledEctoplasm(),
            ItemType.BrokenClock => new BrokenClock(),
            ItemType.GemOfStorm => new GemOfStorm(),
            ItemType.DarkRune => new DarkRune(),
            ItemType.AmuletOfFearAura => new AmuletOfFearAura(),
            ItemType.MysticBook => new MysticBook(),
            ItemType.ScrollOfCurse => new ScrollOfCurse(),
            ItemType.HeartOfGhost => new HeartOfGhost(),
            ItemType.ScareTotem => new ScareTotem(),
            ItemType.GoldLoaf => new GoldLoaf(),
            ItemType.OldKey => new OldKey(),
            ItemType.TotemOfFearEssence => new TotemOfFearEssence(),
            ItemType.None => new NoneItem(),
            ItemType.SoftCurrency => new SoftCurrency(),
            ItemType.HardCurrency => new HardCurrency(),
                    _ => null
        };
}