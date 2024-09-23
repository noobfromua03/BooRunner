using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour, IWindowUI
{
    [SerializeField] private TextMeshProUGUI goals;
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    [SerializeField] private Image musicBtn;
    [SerializeField] private Image soundBtn;

    [SerializeField] private Sprite musicON;
    [SerializeField] private Sprite musicOFF;
    [SerializeField] private Sprite soundON;
    [SerializeField] private Sprite soundOFF;

    private void OnEnable()
    {
        Time.timeScale = 0;
        goals.text = PlayerData.Instance.SetGoals();
        soundBtn.sprite = Progress.Options.Sound ? soundON : soundOFF;
        musicBtn.sprite = Progress.Options.Music ? musicON : musicOFF;
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void SoundBtn()
    {
        Progress.Options.Sound = !Progress.Options.Sound;
        soundBtn.sprite = Progress.Options.Sound ? soundON : soundOFF;
        AudioManager.Instance.ChangeSoundVolume();
    }
    public void MusicBtn()
    {
        Progress.Options.Music = !Progress.Options.Music;
        musicBtn.sprite = Progress.Options.Music ? musicON : musicOFF;
        AudioManager.Instance.ChangeMusicVolume();
    }



    public void ContinueBtn()
        => WindowsManager.Instance.ClosePopup(this);

    public void ExitBtn()
    {
        WindowsManager.Instance.ClosePopup(this);
        PlayerData.Instance.GameOver?.Invoke();
    }

    public void DestroySelf()
        => Destroy(Window);
}