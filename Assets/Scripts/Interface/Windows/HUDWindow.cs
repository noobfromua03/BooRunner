using System;
using System.Collections.Generic;
using UnityEngine;
public class HUDWindow : MonoBehaviour, IWindowUI
{
    [field: SerializeField] public WindowType Type { get; private set; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private List <SlotItem> slots;

    public Action<int> Click;

    public void InitializeButtons(List<ItemType> items)
    {
        if(items.Count > 0)
        {
            for (int i = 0; i < slots.Count; i++)
            {
                var item = ItemBuilder.GetInventoryItem(items[i]);
                slots[i].Initialize(item);
                slots[i].Click += UseSlot;
            }
        }
    }

    public void UpdateSlotsAmount()
    {
        foreach (var slot in slots)
            slot.UpdateAmount();
    }

    private void UseSlot(SlotItem slot)
    {
        Click?.Invoke(slots.IndexOf(slot));
    }

    public void PauseBtn()
        => WindowsManager.Instance.OpenPopup(WindowType.PausePopap);

    public void DestroySelf()
        => Destroy(Window);
}