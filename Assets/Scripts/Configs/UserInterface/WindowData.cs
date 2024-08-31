using UnityEngine;
using UnityEngine.AddressableAssets;

[System.Serializable]
public class WindowData
{
    [field: SerializeField] public WindowType Type { get; private set; }
    [field: SerializeField] public AssetReferenceGameObject Prefab { get; private set; }

    public GameObject RealizeWindow()
        => AddressableExtensions.GetAsset(Prefab);
}

public enum WindowType
{
    MainMenu = 0,
    OptionsPopup = 1,
    PausePopap = 2,
    FortuneWheel = 3,
    ClaimRewardPopup = 4,
    InventoryPopup = 5,
    LevelChoosePopup = 6,
    LevelGoalsPopup = 7,
    Shop = 8,
    TutorialPopup = 9,
    HUD = 10,
    Hero = 11,
    ClaimWheelRewardPopup = 12
}

