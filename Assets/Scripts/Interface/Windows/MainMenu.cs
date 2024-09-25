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
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenPopup(WindowType.LevelChoosePopup);
    }
    public void ShopBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenWindow(WindowType.Shop);
    }
    public void InventoryBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenPopup(WindowType.InventoryPopup);
    }
    public void HeroBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.ChangeCameraView(WindowType.Hero);
        WindowsManager.Instance.OpenWindow(WindowType.Hero);
    }
    public void FortuneWheelBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenWindow(WindowType.FortuneWheel);
    }
    public void TutorialBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenPopup(WindowType.TutorialPopup);
    }
    public void OptionsBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.OpenPopup(WindowType.OptionsPopup);
    }
    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Application.Quit();
    }
    public void DestroySelf()
        => Destroy(Window);
}