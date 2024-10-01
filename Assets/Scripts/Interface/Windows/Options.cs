using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private Image musicBtn;
    [SerializeField] private Image soundBtn;

    [SerializeField] private Sprite musicON;
    [SerializeField] private Sprite musicOFF;
    [SerializeField] private Sprite soundON;
    [SerializeField] private Sprite soundOFF;

    private void Start()
    {
        soundBtn.sprite = Progress.Options.IsSoundOn ? soundON : soundOFF;
        musicBtn.sprite = Progress.Options.IsMusicOn ? musicON : musicOFF;
    }

    public void SoundBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Progress.Options.IsSoundOn = !Progress.Options.IsSoundOn;
        soundBtn.sprite = Progress.Options.IsSoundOn ? soundON : soundOFF;
        AudioManager.Instance.ChangeSoundVolume();
    }
    public void MusicBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Progress.Options.IsMusicOn = !Progress.Options.IsMusicOn;
        musicBtn.sprite = Progress.Options.IsMusicOn ? musicON : musicOFF;
        AudioManager.Instance.ChangeMusicVolume();
    }
    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.ClosePopup(this);
    }

    public void DestroySelf()
        => Destroy(Window);
}