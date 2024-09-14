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

            if (Progress.Inventory.ItemExist(type))
            {
                IInventoryItem item;
                if (Progress.Inventory.items.Find(i => i.Type == type).Amount > 0)
                    item = ItemBuilder.GetInventoryItem(type);
                else
                    item = ItemBuilder.GetInventoryItem(ItemType.None);

                if (item.SubType == ItemSubType.passive && item.Type != ItemType.None)
                {
                    item.ActionHandler();
                    Progress.Inventory.RemoveItem(type);
                }
            }
            OnItemUsed?.Invoke();
        }
    }

    public bool TryUseItem(int index)
    {
        var itemType = items[index];
        if (itemType == ItemType.None)
            return false;

        if (Progress.Inventory.ItemExist(itemType))
        {
            var item = ItemBuilder.GetInventoryItem(itemType);
            if (item.SubType == ItemSubType.active)
            {
                if (item.ActionHandler() == false)
                    return false;
                Progress.Inventory.RemoveItem(itemType);
                OnItemUsed?.Invoke();
                return true;
            }
        }
        return false;
    }

    public List<ItemType> GetSlotTypes() => items;
}
