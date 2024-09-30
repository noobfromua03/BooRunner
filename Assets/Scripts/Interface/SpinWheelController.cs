using System.Collections;
using System.Collections.Generic;
using RandomGeneratorWithWeight;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpinWheelController : MonoBehaviour
{
    [SerializeField]
    private FortuneWheel view;

    [SerializeField]
    private RectTransform wheel;

    [SerializeField]
    private AnimationCurve spinCurve;

    [SerializeField]
    private AnimationCurve finalPosOffsetCurve;

    private const int SECTIONS_AMOUNT = 8;
    private const float FULL_WHEEL = 360f;
    private const float SECTION_CENTER_MIDDLE = FULL_WHEEL / SECTIONS_AMOUNT / 2;
    private const float START_OFFSET = SECTION_CENTER_MIDDLE;
    private (float, float) START_SPEED = (2f, 5f);
    private (float, float) CONST_ROTANIN_TIME = (0.5f, 1.5f);
    private (float, float) FINAL_POS_OFFSET = (
        -SECTION_CENTER_MIDDLE * 0.95f - START_OFFSET,
        SECTION_CENTER_MIDDLE * 0.95f - START_OFFSET
    );
    private (int, int) END_ADDITIONAL_WHEELS_COUNT = (4, 8);

    private Coroutine spinRoutine;
    private RewardItemData rewardItem;
    private AudioSource spinSound;
    public bool IsSpinning { get; private set; }

    private void OnEnable()
    {
        view.Spin += SpinWheel;
        view.RewardSpin += RewardSpinWheel;
        view.StopSpin += StopSpin;
        view.InitializeWheelRewards(RewardConfig.Instance.GetAllWheelRewards());

        UpdateSpins();
    }

    private void OnDisable()
    {
        view.Spin -= SpinWheel;
        view.RewardSpin -= RewardSpinWheel;
        view.StopSpin -= StopSpin;
    }

    private void SpinWheel()
    {
        if (spinRoutine != null)
            return;

        if (Progress.Inventory.spins.spins > 0)
            Play();
    }

    private void RewardSpinWheel()
    {
        if (spinRoutine != null)
            return;

        Debug.Log("Ckicked reward spin");
        if (Progress.Inventory.spins.rewardSpins > 0)
        {
            AudioManager.Instance.PlayAudioByType(AudioType.ButtonClick, AudioSubType.Sound);
            AdvertWrapper.Instance.ShowRewardedVideo(
                "",
                (showed) =>
                {
                    if (showed && Progress.Inventory.spins.rewardSpins > 0)
                    {
                        Play();
                        SpinService.RemoveRewardSpin();
                    }
                },
                () => { }
            );
        }
    }

    private void Play()
    {
        IsSpinning = true;
        UpdateSpins();
        var reward = RewardConfig.Instance.GetWheelReward();
        rewardItem = reward.Item1;

        view.spinBtn.enabled = false;
        view.rewardSpinBtn.enabled = false;
        view.BlockPanel.SetActive(true);
        spinRoutine = StartCoroutine(SpinWheelCoroutine(reward.Item2, rewardItem));
        spinSound = AudioManager.Instance.PlayAudioByType(
            AudioType.FortuneWheel,
            AudioSubType.Music
        );
    }

    private IEnumerator SpinWheelCoroutine(int section, RewardItemData item)
    {
        var currentWheelPos = 0f;
        var speed = Random.Range(START_SPEED.Item1, START_SPEED.Item2);
        var rotationTime = Random.Range(CONST_ROTANIN_TIME.Item1, CONST_ROTANIN_TIME.Item2);
        var timer = 0f;

        while (rotationTime > timer)
        {
            var nextWheelPos = currentWheelPos + speed * Time.deltaTime;
            currentWheelPos = nextWheelPos > 1f ? nextWheelPos - 1f : nextWheelPos;
            wheel.localEulerAngles = new Vector3(0f, 0f, currentWheelPos * FULL_WHEEL);
            timer += Time.deltaTime;
            yield return null;
        }
        var finalSectionPos =
            section * (FULL_WHEEL / SECTIONS_AMOUNT)
            + SECTION_CENTER_MIDDLE
            + CalculateRandomOffset();
        var finalPos =
            finalSectionPos / FULL_WHEEL
            + Random.Range(END_ADDITIONAL_WHEELS_COUNT.Item1, END_ADDITIONAL_WHEELS_COUNT.Item2);
        var distance = finalPos - currentWheelPos;
        var currentDistance = 0f;
        var maxSpeed = speed;

        while (distance > currentDistance)
        {
            var speedMultiplier = spinCurve.Evaluate(currentDistance / distance);
            speed = Mathf.Lerp(0, maxSpeed, speedMultiplier);
            currentDistance += speed * Time.deltaTime;
            currentDistance = Mathf.Min(currentDistance, distance);
            wheel.localEulerAngles = new Vector3(
                0f,
                0f,
                (currentWheelPos + currentDistance) * FULL_WHEEL
            );
            yield return null;
        }

        OnFinishSpin(section, item);
    }

    private void OnFinishSpin(int section, RewardItemData item)
    {
        StopSpin();
        spinSound.Stop();
        Debug.Log(
            "Looted item: "
                + item.Amount
                + " "
                + item.Item.Type.ToString()
                + " with index "
                + section
        );

        var popup =
            WindowsManager.Instance.OpenPopup(WindowType.ClaimRewardPopup) as ClaimRewardPopup;
        if (item.Type == ItemType.SoftCurrency)
            CurrencyService.AddCurrency(CurrencyType.Soft, item.Amount);
        else if (item.Type == ItemType.HardCurrency)
            CurrencyService.AddCurrency(CurrencyType.Hard, item.Amount);
        else
            Progress.Inventory.AddItem(item.Type, item.Amount);

        view.BlockPanel.SetActive(false);

        popup.InitializeReward(item);

        UpdateSpins();
    }

    private float CalculateRandomOffset()
    {
        List<ItemForRandom<float>> randomizeList = new List<ItemForRandom<float>>();

        for (float i = 0; i < 1f; i += 0.01f)
            randomizeList.Add(
                new ItemForRandom<float>()
                    .WithItem(i)
                    .WithWeight((int)(finalPosOffsetCurve.Evaluate(i) * 100))
            );

        var randomFloat = GetItemWithWeight.GetItem(randomizeList);

        return Mathf.Lerp(FINAL_POS_OFFSET.Item1, FINAL_POS_OFFSET.Item2, randomFloat);
    }

    private void UpdateSpins()
    {
        if (SpinService.CompareDate() == false)
        {
            SpinService.ReloadSpinsOnNewDay();
            SpinService.SetDate();
        }

        if (Progress.Inventory.spins.spins == 0)
            view.spins.text = "Today free spin is used";
        if (Progress.Inventory.spins.rewardSpins == 0)
            view.rewardSpins.text = "Today all attempts are used";
    }

    private void StopSpin()
    {
        if (spinRoutine != null)
            StopCoroutine(spinRoutine);
        spinRoutine = null;
        IsSpinning = false;
        view.spinBtn.enabled = true;
        view.rewardSpinBtn.enabled = true;
    }
}
