using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] private GameObject hat;
    private int currentHutIndex = 0;

    private void Start()
    {
        InitializeCustomItems();
    }

    public void InitializeCustomItems()
    {
        if (currentHutIndex == Progress.Inventory.customItems.HatIndex)
            return;

        var hatPrefab = CustomItemConfig.Instance.GetCustomItemByIndex(CustomItemType.Hat,
            Progress.Inventory.customItems.HatIndex);
        if (hatPrefab.IconType != IconType.None)
        {
            DestroyHat(hat.transform);
            Instantiate(hatPrefab.RealizeItem(), hat.transform);
        }
        else
            DestroyHat(hat.transform);

        currentHutIndex = Progress.Inventory.customItems.HatIndex;
    }

    private void DestroyHat(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }
}

