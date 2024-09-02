using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Item : MonoBehaviour
{
    public InterfaceItemType Type => InterfaceItemType.InventoryItem;

    public ItemType ItemType;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private TextMeshProUGUI nameText;

    public Action<ItemType> Click;

    public void OnClick()
    {
        Click?.Invoke(ItemType);
    }

    public void Initialize(IInventoryItem item)
    {
        ItemType = item.Type;
        itemImage.sprite = IconsConfig.Instance.GetByType(item.IconType);
        amountText.text = Progress.Inventory.GetItemAmount(item.Type).ToString();
        nameText.text = item.Type.ToString();
    }
}

