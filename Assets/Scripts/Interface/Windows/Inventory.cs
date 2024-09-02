using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.AddressableAssets.BuildReportVisualizer;
using UnityEngine;

public class Inventory : MonoBehaviour, IWindowUI
{
    [field: SerializeField] public WindowType Type { get; private set; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private Transform content;
    [SerializeField] private Item Slot1;
    [SerializeField] private Item Slot2;

    private List<Item> inventoryItems = new();

    private Item activeSlot;
    private bool IsActiveSlot;

    private void Start()
    {
        InitializeAllItems();
    }

    private void InitializeAllItems()
    {
        var allItems = Progress.Inventory.items;
        var prefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.InventoryItem);

        for (int i = 0; i < allItems.Count; i++)
        {
            var bagItem = InventoryItemConfig.Instance.GetItemByType(allItems[i].Type);

            if (bagItem.Type == ItemType.None)
                continue;

            var inventoryItem = Instantiate(prefab, content);
            var item = inventoryItem.GetComponent<Item>();

            inventoryItems.Add(item);
            item.Click += ClickOnItem;
            if (allItems[i].Amount > 0)
                item.Initialize(bagItem);
        }

        InitializeSlots();
    }

    public void InitializeSlots()
    {
        var currentSlot1 = InventoryItemConfig.Instance.GetItemByType(Progress.Inventory.currentItem_0);
        Slot1.Initialize(currentSlot1);

        var currentSlot2 = InventoryItemConfig.Instance.GetItemByType(Progress.Inventory.currentItem_1);
        Slot2.Initialize(currentSlot2);
    }

    public void ChooseSlot(Item item)
    {
        activeSlot = item;
        IsActiveSlot = true;
    }

    public void ClickOnItem(ItemType type)
    {
        if (IsActiveSlot)
        {
            var inventoryItem = InventoryItemConfig.Instance.GetItemByType(type);
            activeSlot.Initialize(inventoryItem);
            IsActiveSlot = false;
            Save();
        }
    }

    public void ClearBtn()
    {
        Slot1.Initialize(InventoryItemConfig.Instance.GetItemByType(ItemType.None));
        Slot2.Initialize(InventoryItemConfig.Instance.GetItemByType(ItemType.None));
        Save();
    }

    public void ExitBtn()
        => WindowsManager.Instance.ClosePopup(this);

    private void Save()
    {
        Progress.Inventory.currentItem_0 = Slot1.ItemType;
        Progress.Inventory.currentItem_1 = Slot2.ItemType;
    }

    public void DestroySelf()
        => Destroy(Window);
}