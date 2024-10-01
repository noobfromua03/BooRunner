using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class HUDWindow : MonoBehaviour, IWindowUI
{
    [field: SerializeField] public WindowType Type { get; private set; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private List<SlotItem> slots;

    public Func<int, bool> Click;

    public void InitializeButtons(List<ItemType> items)
    {
        if (items.Count > 0)
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
        if (slot.Amount == 0)
            return;

        bool isActionDone = Click?.Invoke(slots.IndexOf(slot)) ?? false;
        if (isActionDone)
        {
            if (slot.ItemType != ItemType.HeartOfGhost)
                StartCoroutine(slot.BlockButton());
            slot.Amount -= 1;
        }
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
    }

    public void PauseBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenPopup(WindowType.PausePopap);
    }

    public void DestroySelf()
        => Destroy(Window);
}