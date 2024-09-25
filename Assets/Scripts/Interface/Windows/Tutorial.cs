using UnityEngine;

public class Tutorial : MonoBehaviour, IWindowUI
{
    [SerializeField] private WindowType type;
    [SerializeField] private SlideInitializer slide;

    private int slideNumber;

    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private void Start()
    {
        slide.Initialize(slideNumber);
    }

    public void ExitBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        WindowsManager.Instance.ClosePopup(this);
    }
    public void PreviousBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        PreviousSlide();
    }
    public void NextBtn()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        NextSlide();
    }
    public void DestroySelf()
        => Destroy(Window);
    private void NextSlide()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        slideNumber += 1;
        if (TutorialConfig.Instance.Slides.Count - 1 < slideNumber)
            slideNumber = 0;
        slide.Initialize(slideNumber);
    }
    private void PreviousSlide()
    {
        AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
        slideNumber -= 1;
        if (slideNumber < 0)
            slideNumber = TutorialConfig.Instance.Slides.Count - 1;
            slide.Initialize(slideNumber);
    }
}