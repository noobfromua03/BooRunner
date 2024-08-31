using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SlideInitializer : MonoBehaviour
{
    [SerializeField] private Image slideImage;
    [SerializeField] private TextMeshProUGUI tutorText;
    public void Initialize(int slideNumber)
    {
        var slideData = TutorialConfig.Instance.Slides[slideNumber];
        slideImage.sprite = slideData.Sprite;
        tutorText.text = slideData.Text;
    }
}

