using UnityEngine;

public class Shop : MonoBehaviour, IWindowUI
{
    [SerializeField] private Transform content;
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private void OnEnable()
    {
        InitializeAllShopItems();
    }

    private void InitializeAllShopItems()
    {
        var allShopItems = ShopItemConfig.Instance.ShopItems;
        var prefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.ShopItem);

        for (int i = 0; i < allShopItems.Count; i++)
        {
            var shopItem = ShopItemConfig.Instance.ShopItems[i];

            var shopItemObject = Instantiate(prefab, content);
            shopItemObject.GetComponent<ShopItemView>().Initialize(shopItem);

        }
    }

    public void ExitBtn()
        => WindowsManager.Instance.OpenWindow(WindowType.MainMenu);

    public void DestroySelf()
        => Destroy(Window);
}