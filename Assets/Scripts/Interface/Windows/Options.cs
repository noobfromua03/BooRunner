using UnityEngine;

public class Options : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    public void SoundBtn()
    {
        Debug.Log("Sound off");
    }
    public void MusicBtn()
    {
        Debug.Log("Music off");
    }
    public void ExitBtn()
        => WindowsManager.Instance.ClosePopup(this);

    public void DestroySelf()
        => Destroy(Window);
}