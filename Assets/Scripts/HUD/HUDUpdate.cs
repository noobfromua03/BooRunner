using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI life;
    [SerializeField] private TextMeshProUGUI score;
    [SerializeField] private TextMeshProUGUI streak;
    [SerializeField] private Slider fearEssence;
    [SerializeField] private Image fearEssenceColor;

    private Color32 green = new Color32(99, 197, 38, 255);
    private Color32 blue = new Color32(63, 116, 224, 255);
    private Color32 red = new Color32(203, 0, 21, 255);
    private Color32 purple = new Color32(152, 55, 202, 255);

    public void UpdateLifes(int value)
    {
        life.text = "x" + value.ToString();
    }

    public void UpdateScore(int value)
    {
        score.text = value.ToString();
    }

    public void UpdateStreak(int value)
    {
        streak.text = "x" + value.ToString();
    }

    public void UpdateFearEssence(int value)
    {
        fearEssence.value = value;

        if (value >= 75)
            fearEssenceColor.color = red;
        else if (value >= 50 && value < 75)
            fearEssenceColor.color = purple;
        else if (value >= 25 && value < 50)
            fearEssenceColor.color = blue;
        else if (value >= 0 && value <= 25)
            fearEssenceColor.color = green;
    }
}

