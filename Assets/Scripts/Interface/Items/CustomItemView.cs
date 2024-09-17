using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class CustomItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject locked;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image currencyIcon;

    public CustomItemData itemData;

    public Action Click;
    public CustomItemType Type;
    public int Index;
    private bool isLocked = false;

    public void OnClick()
    {
        if (isLocked && CanBuyItem())
        {
            var popup = WindowsManager.Instance.OpenPopup(WindowType.ConfirmBuyingPopup) as ConfirmBuyingPopup;
            popup.InitializeCustomItem(this);
        }
        else if(isLocked == false)
        {
            Progress.Inventory.customItems.HatIndex = Index;
            Click?.Invoke();
        }   
    }

    public void Unlock()
        => locked.SetActive(false);

    public void Destroy()
        => Destroy(gameObject);

    public void Initialize(CustomItemType type, int index)
    {
        Index = index;
        itemData = CustomItemConfig.Instance.GetCustomItemByIndex(type, index);
        InitializeIcon();
    }
    public void InitializeIcon()
        => icon.sprite = IconsConfig.Instance.GetByType(itemData.IconType);
    public void InitializeLocked()
    {
        locked.SetActive(true);
        isLocked = true;
        priceText.text = itemData.Price.ToString();

        switch (itemData.CurrencyType)
        {
            case CurrencyType.Soft:
                currencyIcon.sprite = IconsConfig.Instance.GetByType(IconType.SoftCurrency);
                break;
            case CurrencyType.Hard:
                currencyIcon.sprite = IconsConfig.Instance.GetByType(IconType.HardCurrency);
                break;
        }
    }

    private bool CanBuyItem()
    {
        switch (itemData.CurrencyType)
        {
            case CurrencyType.Soft:
                if (CurrencyService.CanBuy(CurrencyType.Soft, itemData.Price))
                    return true;
                break;
            case CurrencyType.Hard:
                if (CurrencyService.CanBuy(CurrencyType.Hard, itemData.Price))
                    return true;
                break;
        }
        return false;
    }
}
