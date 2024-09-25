using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FortuneWheel : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private List<RewardDataView> wheelRewards;
    [SerializeField] public Button spinBtn;
    [SerializeField] public Button rewardSpinBtn;
    [SerializeField] public TextMeshProUGUI spins;
    [SerializeField] public TextMeshProUGUI rewardSpins;
    [SerializeField] public GameObject BlockPanel;

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
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Spin?.Invoke();
        SpinService.RemoveSpin();
    }
    public void AdsBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        AdvertService.ShowRewarded(RewardSpin, () => WindowsManager.Instance.OpenWindow(WindowType.MainMenu));
        SpinService.RemoveRewardSpin();
    }
    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        StopSpin?.Invoke();
        WindowsManager.Instance.OpenWindow(WindowType.MainMenu);
    }
    public void DestroySelf()
        => Destroy(Window);
}