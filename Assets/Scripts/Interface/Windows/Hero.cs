using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private Transform content;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private List<CustomItemView> customItems = new();

    private void OnEnable()
    {
        InitializeItemsByType(CustomItemType.Hat);
    }

    private void InitializeItemsByType(CustomItemType type)
    {
        DestroyContentViews();

        var collection = CustomItemConfig.Instance.CustomItems.Find(c => c.Type == type).Collection;
        var viewPrefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.CustomItem);

        for(int i = 0; i < collection.Count; i++)
        {
            var customItem = Instantiate(viewPrefab, content);
            var customItemView = customItem.GetComponent<CustomItemView>();
            customItemView.Index = i;

            customItems.Add(customItemView);
        }
    }

    private void DestroyContentViews()
    {
        if (customItems.Count == 0)
            return;

        for (int i = customItems.Count - 1; i >= 0; i--)
            customItems[i].Destroy();
        customItems.Clear();
    }
    public void HatsBtn()
        => InitializeItemsByType(CustomItemType.Hat);

    public void ExitBtn()
    {
        WindowsManager.Instance.ChangeCameraView(WindowType.MainMenu);
        WindowsManager.Instance.OpenWindow(WindowType.MainMenu);
    }

    public void DestroySelf()
        => Destroy(Window);

}