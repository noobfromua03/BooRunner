
using RandomGeneratorWithWeight;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinWheelController : MonoBehaviour
{

    [SerializeField] private RectTransform wheel;
    [SerializeField] private AnimationCurve spinCurve;
    [SerializeField] private AnimationCurve finalPosOffsetCurve;

    private const int SECTIONS_AMOUNT = 8;
    private const float FULL_WHEEL = 360f;
    private const float SECTION_CENTER_MIDDLE = FULL_WHEEL / SECTIONS_AMOUNT / 2;
    private const float START_OFFSET = SECTION_CENTER_MIDDLE;
    private (float, float) START_SPEED = (2f, 5f);
    private (float, float) CONST_ROTANIN_TIME = (0.5f, 1.5f);
    private (float, float) FINAL_POS_OFFSET = (-SECTION_CENTER_MIDDLE * 0.95f - START_OFFSET,
        SECTION_CENTER_MIDDLE * 0.95f - START_OFFSET);
    private (int, int) END_ADDITIONAL_WHEELS_COUNT = (4, 8);

    private Coroutine spinRoutine;
    private RewardItemData rewardItem;

    public bool IsSpinning { get; private set; }

    private void OnEnable()
    {
        // звернення до контроллера UI колеса для підписки на SpinWheel
        // оновлення слотів колеса

        // оновлення ціни спіна та кнопки
    }

    private void OnDisable()
    {
        // сказування підписки на SpinWheel, але чому немає дужок
    }

    private void SpinWheel()
    {
        if (spinRoutine != null)
            return;

        /* if(сніпів вистачає) 
         { 
             використати можливий спін
         }*/
    }

    private void Play()
    {
        IsSpinning = true;
        // не розумію що відбувається
        // оновлення кнопки та вартості спіна
        var reward = RewardsConfig.Instance.GetWheelReward();
        rewardItem = reward.Item1;
        // вимкнення якихось додатків
        // вимкнення активності кнопки, яка запускає спін
        spinRoutine = StartCoroutine(SpinWheelCoroutine(reward.Item2, rewardItem)); // чому тут
                                                                                      // амаунт від верхнього а не поточного??
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
            yield return null;
        }

        var finalSectionPos = section * (FULL_WHEEL / SECTIONS_AMOUNT) + SECTION_CENTER_MIDDLE + CalculateRandomOffset();
        var finalPos = finalSectionPos / FULL_WHEEL + Random.Range(END_ADDITIONAL_WHEELS_COUNT.Item1, END_ADDITIONAL_WHEELS_COUNT.Item2);
        var distance = finalPos - currentWheelPos;
        var currentDistance = 0f;
        var maxSpeed = speed;

        while (distance > currentDistance)
        {
            var speedMultiplier = spinCurve.Evaluate(currentDistance / distance);
            speed = Mathf.Lerp(0, maxSpeed, speedMultiplier);
            currentDistance += speed * Time.deltaTime;
            currentDistance = Mathf.Min(currentDistance, distance);
            wheel.localEulerAngles = new Vector3(0f, 0f, (currentWheelPos + currentDistance) * FULL_WHEEL);
            yield return null;
        }

        OnFinishSpin(section, item);
    }

    private void OnFinishSpin(int section, RewardItemData item)
    {
        spinRoutine = null;

        IsSpinning = false;
    }


    private float CalculateRandomOffset()
    {
        List<ItemForRandom<float>> randomizeList = new List<ItemForRandom<float>>();

        for (float i = 0; i < 1f; i += 0.01f)
            randomizeList.Add(new ItemForRandom<float>().WithItem(i).WithWeight((int)(finalPosOffsetCurve.Evaluate(i) * 100)));

        var randomFloat = GetItemWithWeight.GetItem(randomizeList);

        return Mathf.Lerp(FINAL_POS_OFFSET.Item1, FINAL_POS_OFFSET.Item2, randomFloat);
    }

    private void UpdateCostAndButton()
    {

    }
}

