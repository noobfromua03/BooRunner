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
    [SerializeField] private PlayerEffectController effectController;

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
    public BoosterStatus IsChillingTouch = new();
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

    private const float COLOR_STEP = 30f;
    private const float INVINCIBLE_TIME = 2f;
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
        SetInvincible(INVINCIBLE_TIME);

        UpdatePlayerLifes?.Invoke(life);
        UpdateStreak?.Invoke(scaredEnemiesStreak);
        AudioManager.Instance.PlayAudioByType(AudioType.Damaged, AudioSubType.Sound);
        effectController.Damaged();
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

        AudioManager.Instance.PlayAudioByType(AudioType.EssenceCatch, AudioSubType.Sound);
        if (UnityEngine.Random.value > 0.92f)
            AudioManager.Instance.PlayAudioByType(AudioType.Boo, AudioSubType.Sound);
    }

    public void RemoveEssence(int value)
    {
        if (IsChillingTouch.Status)
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
        StartCoroutine(InvincibleDuration(time, () => IsInvincible = false));
    }

    public void ImmaterialityBooster()
    {
        if (IsImmateriality.Status)
            StopCoroutine(IsImmateriality.Coroutine);

        IsImmateriality.Status = true;

        //UpdateBoosterIcon(IconType.Immateriality, BOOSTERS_TIME);
        effectController.Immateriality();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.Immateriality, AudioSubType.Sound);
        IsImmateriality.Coroutine = StartCoroutine(BoosterDuration(() => IsImmateriality.Status = false, audioSource));
    }

    public void SlowMotionsON()
    {
        if (IsSlowMotion.Status)
            StopCoroutine(IsSlowMotion.Coroutine);

        IsSlowMotion.Status = true;
        UpdateStreak?.Invoke(scaredEnemiesStreak);
        //UpdateBoosterIcon(IconType.SlowMotion, BOOSTERS_TIME);
        effectController.SlowMotion();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.SlowMotion, AudioSubType.Sound);
        IsSlowMotion.Coroutine = StartCoroutine(BoosterDuration(() => IsSlowMotion.Status = false,
            UpdateStreak, scaredEnemiesStreak, audioSource));
    }

    public void DarkCloudON()
    {
        if (IsDarkCloud.Status)
            StopCoroutine(IsDarkCloud.Coroutine);

        IsDarkCloud.Status = true;
        //UpdateBoosterIcon(IconType.DarkCloud, BOOSTERS_TIME);
        effectController.DarkCloud();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.DarkCloud, AudioSubType.Sound);
        IsDarkCloud.Coroutine = StartCoroutine(BoosterDuration(() => IsDarkCloud.Status = false, audioSource));
    }

    public void ChillingTouchON()
    {
        if (IsChillingTouch.Status)
            StopCoroutine(IsChillingTouch.Coroutine);

        IsChillingTouch.Status = true;
        //UpdateBoosterIcon(IconType.ChillingTouch, BOOSTERS_TIME);
        effectController.ChillingTouch();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.ChillingTouch, AudioSubType.Sound);
        IsChillingTouch.Coroutine = StartCoroutine(BoosterDuration(() => IsChillingTouch.Status = false, audioSource));
    }

    public void PhantomOfTheOperaON()
    {
        if (IsPhantomOfTheOpera.Status)
            StopCoroutine(IsPhantomOfTheOpera.Coroutine);

        IsPhantomOfTheOpera.Status = true;
        //UpdateBoosterIcon(IconType.PhantomOfTheOpera, BOOSTERS_TIME);
        effectController.PhantomOfTheOpera();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.PhantomOfTheOpera, AudioSubType.Sound);
        IsPhantomOfTheOpera.Coroutine = StartCoroutine(BoosterDuration(() => IsPhantomOfTheOpera.Status = false, audioSource));
    }

    public void TownLegendON()
    {
        if (IsTownLegend.Status)
            StopCoroutine(IsTownLegend.Coroutine);

        IsTownLegend.Status = true;
        //UpdateBoosterIcon(IconType.TownLegend, BOOSTERS_TIME);
        effectController.TownLegend();
        AudioManager.Instance.PlayAudioByType(AudioType.Booster, AudioSubType.Sound);
        var audioSource = AudioManager.Instance.PlayAudioByType(AudioType.TownLegend, AudioSubType.Sound);
        IsTownLegend.Coroutine = StartCoroutine(BoosterDuration(() => IsTownLegend.Status = false, audioSource));
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
    public IEnumerator InvincibleDuration(float time, Action boosterStatus)
    {
        yield return new WaitForSeconds(time);
        boosterStatus?.Invoke();
    }

    public IEnumerator BoosterDuration(Action boosterStatus, AudioSource audioSource)
    {
        yield return new WaitForSeconds(BOOSTERS_TIME);
        boosterStatus?.Invoke();
        audioSource.Stop();
    }
    public IEnumerator BoosterDuration(Action boosterStatus, Action<int> action, int value, AudioSource audioSource)
    {
        yield return new WaitForSeconds(BOOSTERS_TIME);
        boosterStatus?.Invoke();
        action?.Invoke(value);
        audioSource.Stop();
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
            if (IsLevelComplete())
            {
                effectController.LevelComplete();
                AudioManager.Instance.PlayAudioByType(AudioType.Salute, AudioSubType.Sound);
                AudioManager.Instance.PlayAudioByType(AudioType.LevelPassed, AudioSubType.Sound);
            }
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
        else if (FearEssence >= 75 && FearEssence < 100)
            ChangeColor(colors[3]);
        else if (FearEssence == 100)
            ChangeColor(colors[4]);

        for (int i = 0; i < MeshRenderer.Length; i++)
            MeshRenderer[i].material.SetColor("_MainColor", playerColor);
    }

    private void ChangeColor(Color32 color)
    {
        playerColor = Vector3.MoveTowards(playerColor.ToVector3(), color.ToVector3(), COLOR_STEP).ToColor32();
    }
}
