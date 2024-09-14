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
        int amount = Progress.Inventory.GetItemAmount(item.Type);

        if (amount <= 0)
        {
            item = ItemBuilder.GetInventoryItem(ItemType.None);
            amount = 0;
        }

        ItemType = item.Type;
        itemImage.sprite = IconsConfig.Instance.GetByType(item.IconType);
        amountText.text = amount.ToString();
        nameText.text = item.Name;
    }
}

