using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private GameObject currentBoosterPrefab;
    [SerializeField] private GameObject boosterSlots;
    [SerializeField] private GameObject levelDone;

    private List<BoosterIconInitializer> boosterIcons = new();

    private Color32 green = new Color32(99, 197, 38, 255);
    private Color32 blue = new Color32(63, 116, 224, 255);
    private Color32 red = new Color32(203, 0, 21, 255);
    private Color32 purple = new Color32(152, 55, 202, 255);


    public void UpdateLifes(int value)
    {
        life.text = value.ToString();
    }

    public void UpdateScore(int value)
    {
        score.text = value.ToString();
    }

    public void UpdateStreak(int value)
    {
        streak.text = value.ToString();
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

    public void UpdateBoosterIcon(IconType type, float time)
    {
        if (boosterIcons.Count == 0)
            CreateNewBoosterIcon(out var icon);

        var activeBooster = boosterIcons.Find(b => b.gameObject.activeSelf == true &&
        b.GetInitializer().GetIconType() == type);

        if (activeBooster != null)
        {
            activeBooster.FadingOff();
            StopCoroutine(activeBooster.Coroutine);
            activeBooster.Coroutine = StartCoroutine(BoosterDuration(activeBooster, time));
            return;
        }

        activeBooster = boosterIcons.Find(i => i.gameObject.activeSelf == false);

        if (activeBooster == null)
        {
            CreateNewBoosterIcon(out var icon);
            activeBooster = icon;
        }

        activeBooster.gameObject.SetActive(true);
        activeBooster.transform.SetSiblingIndex(boosterIcons.Count - 1);
        activeBooster.GetInitializer().Initialize(type);
        activeBooster.Coroutine = StartCoroutine(BoosterDuration(activeBooster, time));
    }

    public void LevelDone()
        => levelDone.SetActive(true);


    public void CreateNewBoosterIcon(out BoosterIconInitializer icon)
    {
        icon = Instantiate(currentBoosterPrefab, boosterSlots.transform).GetComponent<BoosterIconInitializer>();
        icon.gameObject.SetActive(false);
        boosterIcons.Add(icon.GetComponent<BoosterIconInitializer>());
    }
    public IEnumerator BoosterDuration(BoosterIconInitializer icon, float time)
    {
        yield return new WaitForSeconds(time - 2);
        icon.Fading();
        yield return new WaitForSeconds(2);
        icon.gameObject.SetActive(false);
    }
}

