using System;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private Transform content;
    [SerializeField] private List<CustomItemSlotView> customItemSlots;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private List<CustomItemView> customItems = new();
    private CustomItemSlotView activeSlot;

    public Action UpdateCustomItems;

    private void OnEnable()
    {
        UpdateCustomItems += WindowsManager.Instance.UpdateCustomization;
        InitializeItemsByType(customItemSlots[0].Type);
    }

    private void InitializeItemsByType(CustomItemType type)
    {
        DestroyContentViews();

        activeSlot = customItemSlots.Find(s => s.Type == type);

        var collection = CustomItemConfig.Instance.CustomItems.Find(c => c.Type == type).Collection;
        var viewPrefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.CustomItem);

        for (int i = 0; i < collection.Count; i++)
        {
            var customItem = Instantiate(viewPrefab, content);
            var customItemView = customItem.GetComponent<CustomItemView>();
            customItemView.Click += UpdateActiveSlotAndHero;
            customItemView.Initialize(type, i);

            customItems.Add(customItemView);

            if (Progress.Inventory.IsCustomItemUnlocked(type, i))
                customItems[i].InitializeLocked();

        }
    }

    private void DestroyContentViews()
    {
        if (customItems.Count == 0)
            return;

        for (int i = customItems.Count - 1; i >= 0; i--)
            customItems[i].Destroy();
        customItems.Clear();
    }
    public void ClickOnSlot(CustomItemSlotView slot)
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        InitializeItemsByType(slot.Type);
    }

    public void UpdateActiveSlotAndHero()
    {
        UpdateCustomItems?.Invoke();
        activeSlot.InitializeIcon();
    }    

    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.ChangeCameraView(WindowType.MainMenu);
        WindowsManager.Instance.OpenWindow(WindowType.MainMenu);
    }

    public void DestroySelf()
        => Destroy(Window);

}