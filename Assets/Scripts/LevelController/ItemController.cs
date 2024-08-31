using System.Collections.Generic;

[System.Serializable]
public  class ItemController
{
    private List<ItemType> items = new();

    public void Initialize(List<ItemType> items)
    {
        this.items = items;
    }

    public void ActivatePassiveItems()
    {
        foreach (var type in items)
        {
            var item = InventoryItemConfig.Instance.GetItemByType(type);
            if (item.SubType == ItemSubType.passive)
            {
                item.ActionHandler();
                // - amount
            }
        }
    }

    public void TryUseItem(int index)
    {
        var itemType = items[index];
        var amount = 1; // взято з прогресу за типом

        if(amount > 0)
        {
            var item = InventoryItemConfig.Instance.GetItemByType(itemType);
            if (item.SubType == ItemSubType.active)
                item.ActionHandler();
            // мінус amount
        }
    }

    public List<ItemType> GetSlotTypes()
        => items;
    
}

