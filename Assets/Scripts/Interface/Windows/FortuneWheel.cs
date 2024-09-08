using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class FortuneWheel : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private List<RewardDataView> wheelRewards;
    [SerializeField] public Button spinBtn;
    [SerializeField] public Button rewardSpinBtn;
    [SerializeField] public TextMeshProUGUI spins;
    [SerializeField] public TextMeshProUGUI rewardSpins;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }
    public Action Spin;
    public Action RewardSpin;
    public Action StopSpin;
    public void InitializeWheelRewards(List<RewardItemData> rewardItems)
    {
        for (int i = 0; i < wheelRewards.Count; i++)
            wheelRewards[i].InitializeOnWheel(rewardItems[i]);
    }
    public void SpinBtn()
    {
        Spin?.Invoke();
        SpinService.RemoveSpin();
    }
    public void AdsBtn()
    {
        AdvertService.ShowRewarded(RewardSpin, () => WindowsManager.Instance.OpenWindow(WindowType.MainMenu));
        SpinService.RemoveRewardSpin();
    }
    public void ExitBtn()
    {
        StopSpin?.Invoke();
        WindowsManager.Instance.OpenWindow(WindowType.MainMenu);
    }
    public void DestroySelf()
        => Destroy(Window);
}