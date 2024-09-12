using UnityEngine;
using UnityEngine.UI;
public class CustomItemView : MonoBehaviour
{
    [SerializeField] private Image icon;
    private CustomItemData itemData;

    public CustomItemType Type;
    public int Index;

    private void Start()
    {
        itemData = CustomItemConfig.Instance.GetCustomItemByIndex(Type, Index);
        InitializeIcon();
    }

    public void OnClick()
    {
        Progress.Inventory.customItems.HatIndex = Index;
    }

    public void Destroy()
        => Destroy(gameObject);
    public void InitializeIcon()
        => icon.sprite = IconsConfig.Instance.GetByType(itemData.IconType);
}
