using UnityEngine;
using UnityEngine.UI;
public class CustomItemSlotView : MonoBehaviour
{
    [SerializeField] private Image icon;
    private CustomItemData itemData;

    public CustomItemType Type;
    public int Index;

    private void Update()
    {
        if (Index != Progress.Inventory.customItems.HatIndex)
        {
            Index = Progress.Inventory.customItems.HatIndex;
            itemData = CustomItemConfig.Instance.GetCustomItemByIndex(Type, Index);
            InitializeIcon();
        }
    }
    private void InitializeIcon()
        => icon.sprite = IconsConfig.Instance.GetByType(itemData.IconType);
}
