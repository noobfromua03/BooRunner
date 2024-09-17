using System;
using UnityEngine;

public class ConfirmBuyingPopup : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private CustomItemView customItemView;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public Action OnComfirm;
    public void InitializeCustomItem(CustomItemView data)
    {
        customItemView.Initialize(data.Type, data.Index);
        OnComfirm += data.Click;
        OnComfirm += data.Unlock;

        if (Progress.Inventory.IsCustomItemUnlocked(data.Type, data.Index))
            customItemView.InitializeLocked();
    }
        
    public void ConfirmBtn()
    {
        switch (customItemView.itemData.CurrencyType)
        {
            case CurrencyType.Soft:
                Progress.Inventory.AddUnlockedCustomItem(customItemView.Type, customItemView.Index);
                CurrencyService.RemoveCurrency(CurrencyType.Soft, customItemView.itemData.Price);
                break;
            case CurrencyType.Hard:
                Progress.Inventory.AddUnlockedCustomItem(customItemView.Type, customItemView.Index);
                CurrencyService.RemoveCurrency(CurrencyType.Hard, customItemView.itemData.Price);
                break;
        }

        Progress.Inventory.customItems.HatIndex = customItemView.Index;
        OnComfirm?.Invoke();
        WindowsManager.Instance.ClosePopup(this);
    }
    public void ExitBtn()
        => WindowsManager.Instance.ClosePopup(this);
    public void DestroySelf()
        => Destroy(Window);
}