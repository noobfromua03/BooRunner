using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class CustomItemConfig : AbstractConfig<CustomItemConfig>
{
    [field: SerializeField] public List<CustomItemCollection> CustomItems { get; private set; }

    public CustomItemData GetCustomItemByIndex(CustomItemType type, int i)
    {
        var collection = CustomItems.Find(c => c.Type == type).Collection;
        return collection[i];
    }
}


[Serializable]

public class CustomItemCollection
{
    [field: SerializeField] public CustomItemType Type { get; private set; }
    [field: SerializeField] public List<CustomItemData> Collection { get; private set; }
}

[Serializable]
public class CustomItemData
{
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }
    [field: SerializeField] public IconType IconType { get; private set; }
    [field: SerializeField] public int Price { get; private set; }
    [field: SerializeField] public CurrencyType CurrencyType { get; private set; }


    public GameObject RealizeItem()
        => AddressableExtensions.GetAsset(Prefab);
}

public enum CustomItemType
{
    Hat = 0
}
