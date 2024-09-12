using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class InterfaceItemData
{
    [field: SerializeField] public InterfaceItemType Type { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }

    public GameObject RealizeItem()
        => AddressableExtensions.GetAsset(Prefab);
    
}

public enum InterfaceItemType
{
    LevelCell = 0,
    InventoryItem = 1,
    RewardItem = 2,
    ShopItem = 3,
    CustomItem = 4
}
