using UnityEngine;

public class Shop : MonoBehaviour, IWindowUI
{

    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void ExitBtn()
        => WindowsManager.Instance.OpenWindow(WindowType.MainMenu);

    public void DestroySelf()
        => Destroy(Window);
}