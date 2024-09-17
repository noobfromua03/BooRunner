using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelChoose : MonoBehaviour, IWindowUI
{
    [SerializeField] private Transform content;
    [SerializeField] private WindowType type;
    public WindowType Type { get => type; }
    public GameObject Window { get => gameObject; }

    private List<LevelCell> levelCells = new();

    private void Awake()
    {
        InitializeAllLevelCells();
    }

    private void InitializeAllLevelCells()
    {
        var number = LevelsConfig.Instance.Levels.Count;
        var interfaceItemPrefab = WindowsConfig.Instance.Windows[0].GetItemByType(InterfaceItemType.LevelCell);

        for (int i = 0; i < number; i++)
        {
            var interfaceItem = Instantiate(interfaceItemPrefab, content);
            bool isLevelComplete = LevelsConfig.Instance.Levels[i].Goals.All(g => g.Complete == true);

            levelCells.Add(interfaceItem.GetComponent<LevelCell>()
                .SetLevelNumber(i)
                .With(l => l.Passed(), i < Progress.Inventory.lastCompleteLevel)
                .With(l => l.Unlock(), i <= Progress.Inventory.lastCompleteLevel));
        }
    }

    public void ExitBtn()
        => WindowsManager.Instance.ClosePopup(this);

    public void DestroySelf()
        => Destroy(Window);
}