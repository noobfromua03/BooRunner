using UnityEngine;

public class FortuneWheel : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void SpinBtn()
        => Debug.Log("spin wheel");//RewardsConfig.Instance.GetWheelReward();
    public void AdsBtn()
        => Debug.Log("show advertisement");
    public void ExitBtn()
        => WindowsManager.Instance.OpenWindow(WindowType.MainMenu);
    public void DestroySelf()
        => Destroy(Window);
}