using System;
using UnityEngine;

[Serializable]
public class GoalsData
{
    [field: SerializeField] public bool Complete { get; private set; }
    [field: SerializeField] public GoalType Type { get; private set; }
    [field: SerializeField] public int GoalValue { get; private set; }
    [field: SerializeField] public string Text { get; private set; }

    public void CompleteLevel(bool isTrue)
        => Complete = isTrue;

    public void CopyData(GoalsData goals, int index)
    {
        Type = goals.Type;
        GoalValue = goals.GoalValue;
        Text = goals.Text;
        Complete = Progress.Inventory.lastCompleteLevel > index;
    }

    public void GoalTextBuilder(GoalType type)
    {
        switch (type)
        {
            case GoalType.CollectEssence:
                Text = "Collect some fear essence:";
                break;
            case GoalType.ScarePersons:
                Text = "Scare some persons:";
                break;
            case GoalType.ScaredStreak:
                Text = "Keep scare streak:";
                break;
            case GoalType.TotalScore:
                Text = "Earn total score:";
                break;
        }
    }
}

public enum GoalType
{
    CollectEssence = 0,
    ScarePersons = 1,
    TotalScore = 2,
    ScaredStreak = 3
}

public static class GoalsTextBuilder
{
    public static string GetGoalTextByType(GoalType type)
        => type switch
        {
            GoalType.CollectEssence => new string("Collect some fear essence:"),
            GoalType.ScarePersons => new string("Scare some persons:"),
            GoalType.ScaredStreak => new string("Keep scare streak:"),
            GoalType.TotalScore => new string("Earn total score:"),
            _ => null
        };
}