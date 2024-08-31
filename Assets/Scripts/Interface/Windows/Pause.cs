using TMPro;
using UnityEngine;

public class Pause : MonoBehaviour, IWindowUI
{
    [SerializeField] private TextMeshProUGUI goals;
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private void OnEnable()
    {
        Time.timeScale = 0;
        goals.text = PlayerData.Instance.SetGoals();
    }

    private void OnDisable()
    {
        Time.timeScale = 1;
    }

    public void ContinueBtn()
        => WindowsManager.Instance.ClosePopup(this);

    public void ExitBtn()
    {
        WindowsManager.Instance.ClosePopup(this);
        WindowsManager.Instance.EndLevel();
    }
    public void SoundBtn()
    {
        Debug.Log("Sound off");
    }
    public void MusicBtn()
    {
        Debug.Log("Music off");
    }

    public void DestroySelf()
        => Destroy(Window);
}