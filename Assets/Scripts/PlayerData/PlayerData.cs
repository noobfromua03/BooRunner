using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PlayerData : MonoBehaviour
{
    public static PlayerData instance;

    [SerializeField] private int life;
    [SerializeField] private int fearEssence;
    [SerializeField] private int score;
    [SerializeField] private int scaredEnemiesStreak;
    [SerializeField] private Animator animator;

    private int maxLifes = 5;
    private int maxEssence = 100;

    public int Life { get => life; }
    public int FearEssence { get => fearEssence; }
    public int Score { get => score; }
    public int ScaredEnemiesStreak { get => scaredEnemiesStreak; }

    public bool IsInvincible { get; private set; }
    //public bool IsImmateriality { get; private set; }
    //public bool IsSlowMotion { get; private set; }
    //public bool IsDarkCloud { get; private set; }
    //public bool IsLightsOFF { get; private set; }
    //public bool IsPhantomOfTheOpera { get; private set; }
    //public bool IsTownLegend { get; private set; }

    public BoosterStatus IsImmateriality = new();
    public BoosterStatus IsSlowMotion = new();
    public BoosterStatus IsDarkCloud = new();
    public BoosterStatus IsLightsOFF = new();
    public BoosterStatus IsPhantomOfTheOpera = new();
    public BoosterStatus IsTownLegend = new();

    public Action<int> UpdatePlayerLifes;
    public Action<int> UpdatePlayerScore;
    public Action<int> UpdateStreak;
    public Action<int> UpdateFearEssence;
    public Action<IconType, float> UpdateBoosterIcon;
    public Action GameOver;

    private Coroutine[] activeBoosters = new Coroutine[3];

    private const float BOOSTERS_TIME = 9f;

    private void Start()
    {
        instance = this;
    }

    public void AddLife(int value)
    {
        life = Mathf.Clamp(life += value, 0, maxLifes);
        UpdatePlayerLifes?.Invoke(life);
    }

    public void RemoveLIfe(int value)
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

    public void AddFearEssence(int value)
    {
        if (IsDarkCloud.Status)
            fearEssence += value;

        fearEssence = Mathf.Clamp(fearEssence += value, 0, maxEssence);
        AddScore(1 * scaredEnemiesStreak != 0 ? scaredEnemiesStreak : 1);
        UpdateFearEssence?.Invoke(fearEssence);
    }

    public void RemoveEssence(int value)
    {
        if (IsLightsOFF.Status)
            value /= 2;

        if (IsPhantomOfTheOpera.Status)
        {
            if (value > fearEssence)
                return;
            value /= 2;
        }

        if (IsTownLegend.Status == false)
            fearEssence = Mathf.Clamp(fearEssence -= value, 0, maxEssence);
        AddScore(scaredEnemiesStreak != 0 ? 20 * scaredEnemiesStreak : 20);
        scaredEnemiesStreak++;

        animator.SetTrigger("Boo");

        UpdateStreak.Invoke(scaredEnemiesStreak);
        UpdateFearEssence?.Invoke(fearEssence);
    }

    public void AddScore(int value)
    {
        score += value;
        UpdatePlayerScore?.Invoke(score);
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
}
