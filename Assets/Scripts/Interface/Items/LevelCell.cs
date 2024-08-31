using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCell : MonoBehaviour
{
    [SerializeField] private Image levelImage;
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Image lockImage;
    [SerializeField] private Sprite completeImage;

    public InterfaceItemType Type => InterfaceItemType.LevelCell;
    public GameObject Item { get => gameObject; }
    public bool Completed { get => complete; }

    private int levelNumber;
    private bool complete = false;
    private bool isOpened = false;

    public void OnClick()
    {
        if (!isOpened && levelNumber != 0)
            return;

        WindowsManager.Instance.OpenPopup(WindowType.LevelGoalsPopup);
        WindowsManager.Instance.PreloadLevel(levelNumber);
    }

    public LevelCell SetLevelNumber(int num)
    {
        text.text = "Level " + (num + 1).ToString();
        levelNumber = num;
        return this;
    }

    public void Unlock()
    {
        isOpened = true;
        lockImage.enabled = false;
    }

    public void Passed()
    {
        complete = true;
        levelImage.sprite = completeImage;
    }
}
