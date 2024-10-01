using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlotItem : MonoBehaviour
{
    public InterfaceItemType Type => InterfaceItemType.InventoryItem;

    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Button button;

    public Action<SlotItem> Click;
    public int Amount;
    public ItemType ItemType;
    private ItemSubType subType;
    public void OnClick()
    {
        if (Amount > 0)
        {
            Click?.Invoke(this);
            UpdateAmount();
        }
    }

    public void Initialize(IInventoryItem item)
    {
        ItemType = item.Type;
        subType = item.SubType;
        itemImage.sprite = IconsConfig.Instance.GetByType(item.IconType);
        GetAmount();
        UpdateAmount();
    }

    public void UpdateAmount()
    {
        if (ItemType == ItemType.None)
            amountText.text = string.Empty;
        else
            amountText.text = Amount.ToString();
    }

    private void GetAmount()
    {
        if (subType == ItemSubType.active)
            Amount = Progress.Inventory.GetItemAmount(ItemType) <= 3 ? Progress.Inventory.GetItemAmount(ItemType) : 3;
        else if (subType == ItemSubType.passive)
            Amount = 1;

        if (Amount == 0)
        {
            var item = ItemBuilder.GetInventoryItem(ItemType.None);
            Initialize(item);
        }
    }

    public IEnumerator BlockButton()
    {
        button.enabled = false;
        yield return new WaitForSeconds(PlayerData.BOOSTERS_TIME);
        button.enabled = true;
    }
}