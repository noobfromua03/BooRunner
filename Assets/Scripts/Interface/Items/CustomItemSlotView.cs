using UnityEngine;
using UnityEngine.UI;
public class CustomItemSlotView : MonoBehaviour
{
    [field: SerializeField] public CustomItemType Type { get; private set; }
    [SerializeField] private Image icon;
    private CustomItemData itemData;

    public int Index;

    private void Start()
    {
        InitializeIcon();
    }
    public void InitializeIcon()
    {
        if (Index != Progress.Inventory.customItems.HatIndex)
        {
            Index = Progress.Inventory.customItems.HatIndex;
            itemData = CustomItemConfig.Instance.GetCustomItemByIndex(Type, Index);
            icon.sprite = IconsConfig.Instance.GetByType(itemData.IconType);
        }

        if (Index == 0)
        {
            icon.sprite = IconsConfig.Instance.GetByType(IconType.CowboyHat);
            return;
        }
    }
}
