using System;
using System.Collections;
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

    [field: SerializeField] public bool IsInvincible { get; private set; }
    [field: SerializeField] public bool IsSlowMotion { get; private set; }
    [field: SerializeField] public bool IsDarkCloud { get; private set; }
    [field: SerializeField] public bool IsLightsOFF { get; private set; }
    [field: SerializeField] public bool IsPhantomOfTheOpera { get; private set; }
    [field: SerializeField] public bool IsTownLegend { get; private set; }

    public Action<int> UpdatePlayerLifes;
    public Action<int> UpdatePlayerScore;
    public Action<int> UpdateStreak;
    public Action<int> UpdateFearEssence;
    public Action GameOver;

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
        if (IsInvincible)
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
        if (IsDarkCloud)
            fearEssence += value;

        fearEssence = Mathf.Clamp(fearEssence += value, 0, maxEssence);
        AddScore(1 * scaredEnemiesStreak != 0 ? scaredEnemiesStreak : 1);
        UpdateFearEssence?.Invoke(fearEssence);
    }

    public void RemoveEssence(int value)
    {
        if (IsLightsOFF)
            value /= 2;

        if(IsPhantomOfTheOpera)
        {
            if (value > fearEssence)
                return;
            value /= 2;
        }

        if (IsTownLegend == false)
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
        if (IsInvincible)
            return;

        SetInvincible(6);
    }

    public void SlowMotionsON()
    {
        if (IsSlowMotion)
            return;

        IsSlowMotion = true;
        UpdateStreak?.Invoke(scaredEnemiesStreak);
        StartCoroutine(BoosterDuration(6, () => IsSlowMotion = false, UpdateStreak, scaredEnemiesStreak));
    }

    public void DarkCloudON()
    {
        if (IsDarkCloud)
            return;

        IsDarkCloud = true;
        StartCoroutine(BoosterDuration(10, () => IsDarkCloud = false));
    }

    public void SetLightsOFF()
    {
        if (IsLightsOFF)
            return;

        IsLightsOFF = true;
        StartCoroutine(BoosterDuration(10, () => IsLightsOFF = false));
    }

    public void PhantomOfTheOperaON()
    {
        if (IsPhantomOfTheOpera)
            return;

        IsPhantomOfTheOpera = true;
        StartCoroutine(BoosterDuration(8, () => IsPhantomOfTheOpera = false));
    }

    public void TownLegendON()
    {
        if (IsTownLegend)
            return;

        IsTownLegend = true;
        StartCoroutine(BoosterDuration(10, () => IsTownLegend = false));
    }

    public IEnumerator BoosterDuration(float time, Action boosterStatus)
    {
        yield return new WaitForSeconds(time);
        boosterStatus?.Invoke();

    }
    public IEnumerator BoosterDuration(float time, Action boosterStatus, Action<int> action, int value)
    {
        yield return new WaitForSeconds(time);
        boosterStatus?.Invoke();
        action?.Invoke(value);
    }
}
