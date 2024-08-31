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
}

public enum GoalType
{
    CollectEssence = 0,
    ScarePersons = 1,
    TotalScore = 2,
    ScaredStreak = 3
}