using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour, IWindowUI
{
    [field: SerializeField] public WindowType Type { get; private set; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private Transform content;
    [SerializeField] private List<Item> Slots;

    private List<Item> inventoryItems = new();

    private Item activeSlot;
    private bool IsActiveSlot;

    private void OnEnable()
    {
        CreateNone();
        InitializeAllItems();
        InitializeSlots();
    }

    private void InitializeAllItems()
    {
        var allItems = Progress.Inventory.items;
        var prefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.InventoryItem);

        for (int i = 0; i < allItems.Count; i++)
        {
            var bagItem = ItemBuilder.GetInventoryItem(allItems[i].Type);

            if (bagItem.Type == ItemType.None || allItems[i].Amount == 0)
                continue;

            var inventoryItem = Instantiate(prefab, content);
            var item = inventoryItem.GetComponent<Item>();

            inventoryItems.Add(item);
            item.Click += ClickOnItem;
            item.Initialize(bagItem);
        }
    }

    private void CreateNone()
    {
        if (Progress.Inventory.ItemExist(ItemType.None, 0) == false)
        {
            var none = ItemBuilder.GetInventoryItem(ItemType.None);
            Progress.Inventory.AddItem(none.Type, 1);
        }
    }



    public void InitializeSlots()
    {
        var currentSlot1 = ItemBuilder.GetInventoryItem(Progress.Inventory.currentItem_0);
        Slots[0].Initialize(currentSlot1);

        var currentSlot2 = ItemBuilder.GetInventoryItem(Progress.Inventory.currentItem_1);
        Slots[1].Initialize(currentSlot2);
    }

    public void ChooseSlot(Item item)
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        activeSlot = item;
        IsActiveSlot = true;
    }

    public void ClickOnItem(ItemType type)
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        if (IsActiveSlot)
        {
            var inventoryItem = ItemBuilder.GetInventoryItem(type);

            if (type == Slots.Find(s => s != activeSlot).ItemType)
            {
                var inventoryItemNone = ItemBuilder.GetInventoryItem(ItemType.None);
                Slots.Find(s => s != activeSlot).Initialize(inventoryItemNone);
            }

            activeSlot.Initialize(inventoryItem);
            IsActiveSlot = false;
            Save();
        }
    }

    public void ClearBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Slots[0].Initialize(ItemBuilder.GetInventoryItem(ItemType.None));
        Slots[1].Initialize(ItemBuilder.GetInventoryItem(ItemType.None));
        Save();
    }

    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.ClosePopup(this);
    }

    private void Save()
    {
        Progress.Inventory.currentItem_0 = Slots[0].ItemType;
        Progress.Inventory.currentItem_1 = Slots[1].ItemType;

        InitializeSlots();
    }

    public void DestroySelf()
        => Destroy(Window);
}