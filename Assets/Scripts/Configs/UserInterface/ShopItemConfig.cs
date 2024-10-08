﻿using System.Collections.Generic;
using UnityEngine;

public class ShopItemConfig : AbstractConfig<ShopItemConfig>
{
    [field: SerializeField] public List<ShopItemData> ShopItems { get; private set; }
}

[System.Serializable]

public class ShopItemData
{
    [field: SerializeField] public ItemType Type { get; private set; }
    public IInventoryItem Item { get => ItemBuilder.GetInventoryItem(Type);}
    [field: SerializeField] public CurrencyType CurrencyType { get; private set; }
    [field: SerializeField] public int Cost { get; private set; }
    [field: SerializeField] public int Amount { get; private set; } = 1;
}