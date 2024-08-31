using UnityEngine;

public class ClaimRewardPopup : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private RewardItemData rewardItemData;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private void OnEnable()
    {
        int value = PlayerData.Instance.GetSoftReward() / 100;
        value = value < 10 ? 10 : value;
        rewardItemData.SetAmount(value);
    }

    public void ClaimBtn()
    {
        WindowsManager.Instance.ClosePopup(this);
        WindowsManager.Instance.EndLevel();
    }

    public void DestroySelf()
        => Destroy(Window);
}