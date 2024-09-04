using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemView : MonoBehaviour
{
    public InterfaceItemType Type => InterfaceItemType.ShopItem;

    private ShopItemData shopItemData;

    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Image itemImage;
    [SerializeField] private Image currencyIcon;

    public void OnClick()
    {
        BuyButtonHandler();

    }

    public void Initialize(ShopItemData shopItem)
    {
        shopItemData = shopItem;
        amountText.text = shopItemData.Amount.ToString();
        nameText.text = shopItemData.Item.Name;
        priceText.text = shopItemData.Cost.ToString();
        itemImage.sprite = IconsConfig.Instance.GetByType(shopItemData.Item.IconType);
        currencyIcon.sprite = CurrencyIconInitializer(shopItemData.CurrencyType);
    }

    private Sprite CurrencyIconInitializer(CurrencyType type)
    {
        if (type == CurrencyType.Soft)
            return IconsConfig.Instance.GetByType(IconType.SoftCurrency);
        else if (type == CurrencyType.Hard)
            return IconsConfig.Instance.GetByType(IconType.HardCurrency);
        else if (type == CurrencyType.Real)
            return IconsConfig.Instance.GetByType(IconType.RealCurrency);
        return null;
    }

    private void BuyButtonHandler()
    {
        if (shopItemData.Type == ItemType.SoftCurrency)
        {
            if (CurrencyService.CanBuy(CurrencyType.Soft, shopItemData.Cost))
            {
                CurrencyService.RemoveCurrency(shopItemData.CurrencyType, shopItemData.Cost);
                CurrencyService.AddCurrency(CurrencyType.Soft, shopItemData.Amount);
            }
        }
        else if (shopItemData.Type == ItemType.HardCurrency)
            return; // тут має бути покупка за РЕАЛЬНЕ БАБЛО!!!!
        else
        {
            if (CurrencyService.CanBuy(shopItemData.CurrencyType, shopItemData.Cost))
            {
                CurrencyService.RemoveCurrency(shopItemData.CurrencyType, shopItemData.Cost);
                Progress.Inventory.AddItem(shopItemData.Type, shopItemData.Amount);
            }
        }
    }
}

