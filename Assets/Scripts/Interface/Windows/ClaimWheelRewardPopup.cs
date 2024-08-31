using UnityEngine;

public class ClaimWheelRewardPopup : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void ClaimBtn()
        => WindowsManager.Instance.ClosePopup(this);

    public void DestroySelf()
        => Destroy(Window);
}