using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelGoalsPopup : MonoBehaviour, IWindowUI
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }
    public void GoBtn()
        => WindowsManager.Instance.StartLevel();
    public void ExitBtn()
        => WindowsManager.Instance.ClosePopup(this);
    public void SetText(int level)
    {
        List<GoalsData> allGoals = LevelsConfig.Instance.Levels[level].Goals;

        text.text = "";
        allGoals.ForEach(g => text.text += g.Text + " " + g.GoalValue.ToString() + "\n");
    }
    public void DestroySelf()
        => Destroy(Window);
}