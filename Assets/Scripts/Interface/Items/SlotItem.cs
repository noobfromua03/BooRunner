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
    public void OnClick()
    {
        Click?.Invoke(this);
    }

    public void Initialize(IInventoryItem item)
    {
        itemImage.sprite = IconsConfig.Instance.GetByType(item.IconType);
        amountText.text = "5"; // progress get amount of item by type
    }
}