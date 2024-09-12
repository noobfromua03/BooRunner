using UnityEngine;

public class PlayerCustomization : MonoBehaviour
{
    [SerializeField] private GameObject hat;
    private int currentHutIndex;

    private void Update()
    {
        if (currentHutIndex != Progress.Inventory.customItems.HatIndex)
        {
            InitializeCustomItems();
            currentHutIndex = Progress.Inventory.customItems.HatIndex;
        }
    }

    private void InitializeCustomItems()
    {
        var hatPrefab = CustomItemConfig.Instance.GetCustomItemByIndex(CustomItemType.Hat,
            Progress.Inventory.customItems.HatIndex);
        if (hatPrefab.IconType != IconType.None)
        {
            DestroyHat(hat.transform);
            Instantiate(hatPrefab.RealizeItem(), hat.transform);
        }
        else
            DestroyHat(hat.transform);

    }

    private void DestroyHat(Transform parent)
    {
        foreach (Transform child in parent)
            Destroy(child.gameObject);
    }
}

