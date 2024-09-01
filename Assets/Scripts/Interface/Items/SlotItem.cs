using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public InterfaceItemType Type => InterfaceItemType.InventoryItem;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;

    public Action<SlotItem> Click;
    private ItemType itemType;
    public void OnClick()
    {
        Click?.Invoke(this);
    }

    public void Initialize(IInventoryItem item)
    {
        itemType = item.Type;

        itemImage.sprite = IconsConfig.Instance.GetByType(item.IconType);
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        amountText.text = Progress.Inventory.GetItemAmount(itemType).ToString();
    }
}