using System.Collections.Generic;

public partial class Progress : ProgressBase<Progress>
{
    [System.Serializable]
    public class InventoryProgress
    {
        public int soft;
        public int hard;
        public ItemType currentItem_0 = ItemType.None;
        public ItemType currentItem_1 = ItemType.None;
        public List<InventoryItem> items = new();

        [System.Serializable]
        public class InventoryItem
        {
            public ItemType Type;
            public int Amount;
        }

        public bool ItemExist(ItemType type, int amount = 1)
            => items.Exists(i => i.Type == type) && GetItemAmount(type) >= amount;

        public int GetItemAmount(ItemType type)
        {
            var item = items.Find(i => i.Type == type);
            if (item == null)
            {
                UnityEngine.Debug.LogError($"Item with type {type} not found in inventory");
                return 0;
            }
            return item.Amount;
        }

        public void AddItem(ItemType type, int amount = 1)
        {
            if (ItemExist(type))
            {
                items.Find(i => i.Type == type).Amount += amount;
                return;
            }

            items.Add(new InventoryItem { Type = type, Amount = amount });
        }

        public void RemoveItem(ItemType type, int amount = 1)
        {
            if (ItemExist(type))
            {
                var currentAmount = GetItemAmount(type);
                if (currentAmount - amount < 0)
                {
                    UnityEngine.Debug.LogError($"Trying to remove more items than available in inventory");
                    return;
                }

                items.Find(i => i.Type == type).Amount -= amount;
                return;
            }
            UnityEngine.Debug.LogError($"Trying to remove item {type} that is not in inventory");
        }
    }
}
