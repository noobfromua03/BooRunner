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
        soundBtn.sprite = Progress.Options.SoundMute ? soundON : soundOFF;
        musicBtn.sprite = Progress.Options.MusicMute ? musicON : musicOFF;
    }

    public void SoundBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Progress.Options.SoundMute = !Progress.Options.SoundMute;
        soundBtn.sprite = Progress.Options.SoundMute ? soundON : soundOFF;
        AudioManager.Instance.ChangeSoundVolume();
    }
    public void MusicBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        Progress.Options.MusicMute = !Progress.Options.MusicMute;
        musicBtn.sprite = Progress.Options.MusicMute ? musicON : musicOFF;
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