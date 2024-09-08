using System;
using UnityEngine;

public class ClaimRewardPopup : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private RewardItemData rewardItemData;
    [SerializeField] private Transform container;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void InitializeReward(RewardItemData data)
    {
        var prefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.RewardItem);
        var rewardItem = Instantiate(prefab, container);
        rewardItem.GetComponent<RewardDataView>().Initialize(data);
    }

    public void ClaimBtn()
    {
        WindowsManager.Instance.ClosePopup(this);
        WindowsManager.Instance.ReturnToMenu();
    }

    public void DestroySelf()
        => Destroy(Window);
}