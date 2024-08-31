using System.Collections.Generic;
using UnityEngine;

public class WindowsConfig : AbstractConfig<WindowsConfig>
{
    [field: SerializeField] public bool CreateOnlyBasicWindowsAtStart { get; private set; }
    [field: SerializeField] public bool DestroyInactiveWindows { get; private set; }

    [field: SerializeField] public List<WindowType> BasicWindows;
    [field: SerializeField] public List<WindowsConfigData> Windows { get; private set; }

}

[System.Serializable]
public class WindowsConfigData
{
    [field: SerializeField] public List<WindowData> WindowPrefabs { get; private set; }
    [field: SerializeField] public List<InterfaceItemData> InterfaceItemPrefabs { get; private set; }

    public GameObject GetWindowByType(WindowType type)
        => WindowPrefabs.Find(w => w.Type == type).RealizeWindow();

    public GameObject GetItemByType(InterfaceItemType type)
        => InterfaceItemPrefabs.Find(i => i.Type == type).RealizeItem();
}

