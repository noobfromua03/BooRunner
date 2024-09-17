using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData Instance;

    [SerializeField] private Animator animator;
    [SerializeField] private SkinnedMeshRenderer[] MeshRenderer;
    [SerializeField] private List<Color32> colors;

    private int life;
    private int scaredEnemiesStreak;

    private int maxLifes = 3;
    private int maxEssence = 100;
    private float currentDissolveValue = 1f;

    private Color32 playerColor;
    public int FearEssence { get; private set; }
    public int Score { get; private set; }


    public bool IsInvincible { get; private set; }
    public bool IsScareTotem { get; private set; }
    public bool IsGoldLoaf { get; private set; }
    public bool IsCoffinKey { get; private set; }
    public bool IsDissolve { get; private set; } = false;

    public Dictionary<GoalsData, int> currentGoals = new();
    private int currentLevelIndex;

    public BoosterStatus IsImmateriality = new();
    public BoosterStatus IsSlowMotion = new();
    public BoosterStatus IsDarkCloud = new();
    public BoosterStatus IsLightsOFF = new();
    public BoosterStatus IsPhantomOfTheOpera = new();
    public BoosterStatus IsTownLegend = new();
    public BoosterStatus IsTotemOfFear = new();

    public Action<int> UpdatePlayerLifes;
    public Action<int> UpdatePlayerScore;
    public Action<int> UpdateStreak;
    public Action<int> UpdateFearEssence;
    public Action<IconType, float> UpdateBoosterIcon;
    public Action GameOver;
    public Action UpdateLevelComplete;

    private const float COLOR_STEP = 15f;
    public const float BOOSTERS_TIME = 6f;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        life = maxLifes;

        IsLevelComplete();
    }

    private void Update()
    {
        //PlayDissolve();
        ColorUpdate();
    }

    public void AddLife(int value)
    {
        life = Mathf.Clamp(life += value, 0, maxLifes);
        UpdatePlayerLifes?.Invoke(life);
    }

    public void RemoveLife(int value)
    {
        if (IsInvincible || IsImmateriality.Status)
            return;

        life -= value;

        if (life <= 0)
            GameOver?.Invoke();

        animator.SetTrigger("Damaged");

        scaredEnemiesStreak = scaredEnemiesStreak <= 1 ? scaredEnemiesStreak : scaredEnemiesStreak / 2;
        SetInvincible(2);

        UpdatePlayerLifes?.Invoke(life);
        UpdateStreak?.Invoke(scaredEnemiesStreak);

    }

    public bool CompareLife()
        => life == maxLifes;

    public void AddFearEssence(int value)
    {
        if (IsDarkCloud.Status)
            value *= 2;

        FearEssence = Mathf.Clamp(FearEssence += value, 0, maxEssence);
        AddScore(1 * scaredEnemiesStreak != 0 ? scaredEnemiesStreak : 1);
        UpdateFearEssence?.Invoke(FearEssence);

        ChangeGoalValueByType(GoalType.CollectEssence, value);
    }

    public void RemoveEssence(int value)
    {
        if (IsLightsOFF.Status)
            value /= 2;

        if (IsPhantomOfTheOpera.Status)
        {
            if (value > FearEssence)
                return;
            value /= 2;
        }

        if (IsTownLegend.Status == false)
            FearEssence = Mathf.Clamp(FearEssence -= value, 0, maxEssence);

        var score = scaredEnemiesStreak != 0 ? 20 * scaredEnemiesStreak : 20;

        if (IsScareTotem)
            score *= 3;

        AddScore(score);

        scaredEnemiesStreak++;

        animator.SetTrigger("Boo");

        UpdateStreak.Invoke(scaredEnemiesStreak);
        UpdateFearEssence?.Invoke(FearEssence);

        ChangeGoalValueByType(GoalType.ScarePersons, 1);
        ChangeGoalValueByType(GoalType.ScaredStreak, scaredEnemiesStreak);

        ColorUpdate();
    }

    public void AddScore(int value)
    {
        Score += value;
        UpdatePlayerScore?.Invoke(Score);

        ChangeGoalValueByType(GoalType.TotalScore, value);
    }

    public void SetInvincible(float time)
    {
        IsInvincible = true;
        StartCoroutine(BoosterDuration(time, () => IsInvincible = false));
    }

    public void ImmaterialityBooster()
    {
        if (IsImmateriality.Status)
            StopCoroutine(IsImmateriality.Coroutine);

        IsImmateriality.Status = true;

        UpdateBoosterIcon(IconType.Immateriality, BOOSTERS_TIME);
        IsImmateriality.Coroutine = StartCoroutine(BoosterDuration(() => IsImmateriality.Status = false));
    }

    public void SlowMotionsON()
    {
        if (IsSlowMotion.Status)
            StopCoroutine(IsSlowMotion.Coroutine);

        IsSlowMotion.Status = true;
        UpdateStreak?.Invoke(scaredEnemiesStreak);
        UpdateBoosterIcon(IconType.SlowMotion, BOOSTERS_TIME);
        IsSlowMotion.Coroutine = StartCoroutine(BoosterDuration(() => IsSlowMotion.Status = false, UpdateStreak, scaredEnemiesStreak));
    }

    public void DarkCloudON()
    {
        if (IsDarkCloud.Status)
            StopCoroutine(IsDarkCloud.Coroutine);

        IsDarkCloud.Status = true;
        UpdateBoosterIcon(IconType.DarkCloud, BOOSTERS_TIME);
        IsDarkCloud.Coroutine = StartCoroutine(BoosterDuration(() => IsDarkCloud.Status = false));
    }

    public void LightsOFF()
    {
        if (IsLightsOFF.Status)
            StopCoroutine(IsLightsOFF.Coroutine);

        IsLightsOFF.Status = true;
        UpdateBoosterIcon(IconType.LightsOff, BOOSTERS_TIME);
        IsLightsOFF.Coroutine = StartCoroutine(BoosterDuration(() => IsLightsOFF.Status = false));
    }

    public void PhantomOfTheOperaON()
    {
        if (IsPhantomOfTheOpera.Status)
            StopCoroutine(IsPhantomOfTheOpera.Coroutine);

        IsPhantomOfTheOpera.Status = true;
        UpdateBoosterIcon(IconType.PhantomOfTheOpera, BOOSTERS_TIME);
        IsPhantomOfTheOpera.Coroutine = StartCoroutine(BoosterDuration(() => IsPhantomOfTheOpera.Status = false));
    }

    public void TownLegendON()
    {
        if (IsTownLegend.Status)
            StopCoroutine(IsTownLegend.Coroutine);

        IsTownLegend.Status = true;
        UpdateBoosterIcon(IconType.TownLegend, BOOSTERS_TIME);

        IsTownLegend.Coroutine = StartCoroutine(BoosterDuration(() => IsTownLegend.Status = false));
    }

    public void ScrollOfCurse()
    {
        scaredEnemiesStreak *= 2;
        UpdateStreak(scaredEnemiesStreak);
    }

    public void ScareTotemON()
        => IsScareTotem = true;

    public void GoldBreadON()
        => IsGoldLoaf = true;

    public void CoffinKeyON()
    {
        maxLifes = 5;
        life = maxLifes;
        UpdatePlayerLifes?.Invoke(maxLifes);
    }

    public void TotemOfFearEssence()
    {
        IsTotemOfFear.Status = true;
        IsTotemOfFear.Coroutine = StartCoroutine(TotemOfFearCoroutine());
    }

    public IEnumerator BoosterDuration(Action boosterStatus)
    {
        yield return new WaitForSeconds(BOOSTERS_TIME);
        boosterStatus?.Invoke();
    }
    public IEnumerator BoosterDuration(float time, Action boosterStatus)
    {
        yield return new WaitForSeconds(time);
        boosterStatus?.Invoke();

    }
    public IEnumerator BoosterDuration(Action boosterStatus, Action<int> action, int value)
    {
        yield return new WaitForSeconds(BOOSTERS_TIME);
        boosterStatus?.Invoke();
        action?.Invoke(value);
    }

    public IEnumerator TotemOfFearCoroutine()
    {
        while (IsTotemOfFear.Status)
        {
            yield return new WaitForSeconds(BOOSTERS_TIME);
            AddFearEssence(10);
        }
    }

    public void GetCurrentGoals(LevelData levelData, int index)
    {
        currentLevelIndex = index;
        foreach (var item in levelData.Goals)
        {
            var goalData = new GoalsData();
            goalData.CopyData(item, index);
            currentGoals.Add(goalData, 0);
        }    
    }

    public string SetGoals()
    {
        string goalsText = "";

        if (IsLevelComplete())
            return goalsText = "Level complete!!!";

        foreach (var item in currentGoals)
        {
            goalsText += $"{item.Key.Text} {item.Value}/{item.Key.GoalValue} \n";
        }

        return goalsText;
    }

    public void ChangeGoalValueByType(GoalType type, int value)
    {
        if (IsLevelComplete())
            return;

        var goal = currentGoals.Keys.ToList().Find(g => g.Type == type);
        if (goal != null)
        {
            if (type == GoalType.ScaredStreak)
                currentGoals[goal] = value;
            else
                currentGoals[goal] += value;

            goal.CompleteLevel(goal.GoalValue <= currentGoals[goal]);
            IsLevelComplete();
        }


    }
    private bool IsLevelComplete()
    {
        if (currentGoals.Keys.All(g => g.Complete))
        {
            Progress.Inventory.SetLastCompleteLevel(currentLevelIndex);
            UpdateLevelComplete?.Invoke();
            currentGoals.Clear();
            return true;
        }
        return false;
    }

    public void Dissolve(bool isDissolve)
    {
        IsDissolve = isDissolve;
    }

    private void PlayDissolve()
    {
        var dissolveValue = 0.025f;

        if (scaredEnemiesStreak < 15)
            dissolveValue *= 2;

        if (IsDissolve)
            currentDissolveValue = Mathf.Clamp(currentDissolveValue - dissolveValue, 0.6f, 1);
        else if (IsDissolve == false && currentDissolveValue != 1)
            currentDissolveValue = Mathf.Clamp(currentDissolveValue + dissolveValue, 0.6f, 1);

        for (int i = 0; i < MeshRenderer.Length; i++)
            MeshRenderer[i].material.SetFloat("_Dissolve", currentDissolveValue);
    }

    private void ColorUpdate()
    {
        if (FearEssence < 25)
            ChangeColor(colors[0]);
        else if (FearEssence >= 25 && FearEssence < 50)
            ChangeColor(colors[1]);
        else if (FearEssence >= 50 && FearEssence < 75)
            ChangeColor(colors[2]);
        else if (FearEssence > 75)
            ChangeColor(colors[3]);

        for (int i = 0; i < MeshRenderer.Length; i++)
            MeshRenderer[i].material.SetColor("_MainColor", playerColor);
    }

    private void ChangeColor(Color32 color)
    {
        playerColor = Vector3.MoveTowards(playerColor.ToVector3(), color.ToVector3(), COLOR_STEP).ToColor32();
    }
}
