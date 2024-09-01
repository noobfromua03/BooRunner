using System;
using System.Collections.Generic;

[System.Serializable]
public class ItemController
{
    private List<ItemType> items = new();

    public Action OnItemUsed;

    public void Initialize()
    {
        items = new List<ItemType>()
        {
            Progress.Inventory.currentItem_0,
            Progress.Inventory.currentItem_1
        };
    }

    public void ActivatePassiveItems()
    {
        foreach (var type in items)
        {
            if (type == ItemType.None)
                continue;

            if (Progress.Inventory.HasItem(type))
            {
                var item = InventoryItemConfig.Instance.GetItemByType(type);
                if (item.SubType == ItemSubType.passive)
                {
                    item.ActionHandler();
                    Progress.Inventory.RemoveItem(type);
                    OnItemUsed?.Invoke();
                }
            }
        }
    }

    public void TryUseItem(int index)
    {
        var itemType = items[index];
        if (itemType == ItemType.None)
            return;

        if (Progress.Inventory.HasItem(itemType))
        {
            var item = InventoryItemConfig.Instance.GetItemByType(itemType);
            if (item.SubType == ItemSubType.active)
            {
                item.ActionHandler();
                Progress.Inventory.RemoveItem(itemType);
                OnItemUsed?.Invoke();
            }
        }
    }

    public List<ItemType> GetSlotTypes() => items;
}
