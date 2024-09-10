using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour, IWindowUI
{
    [SerializeField] private TextMeshProUGUI lifes;
    [SerializeField] private TextMeshProUGUI softCurrency;
    [SerializeField] private TextMeshProUGUI hardCurrency;
    [SerializeField] private TextMeshProUGUI lifesTimer;

    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void PlayBtn()
        => WindowsManager.Instance.OpenPopup(WindowType.LevelChoosePopup);
    public void ShopBtn()
        => WindowsManager.Instance.OpenWindow(WindowType.Shop);
    public void InventoryBtn()
        => WindowsManager.Instance.OpenPopup(WindowType.InventoryPopup);
    public void HeroBtn()
    {
        WindowsManager.Instance.ChangeCameraView(WindowType.Hero);
        WindowsManager.Instance.OpenWindow(WindowType.Hero);
    }
    public void FortuneWheelBtn()
        => WindowsManager.Instance.OpenWindow(WindowType.FortuneWheel);
    public void TutorialBtn() 
        => WindowsManager.Instance.OpenPopup(WindowType.TutorialPopup);
    public void OptionsBtn() 
        => WindowsManager.Instance.OpenPopup(WindowType.OptionsPopup);
    public void ExitBtn()
        => Application.Quit();
    public void DestroySelf()
        => Destroy(Window);
}