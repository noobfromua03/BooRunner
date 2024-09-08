using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RewardDataView : MonoBehaviour
{
    private RewardItemData ItemData;

    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;
    [SerializeField] private Image icon;

    public void Initialize(RewardItemData data)
    {
        ItemData = data;
        itemName.text = data.Item.Name;
        amount.text = data.Amount.ToString();
        icon.sprite = IconsConfig.Instance.GetByType(data.Item.IconType);
    }

    public void InitializeOnWheel(RewardItemData data)
    {
        ItemData = data;
        amount.text = data.Amount.ToString();
        icon.sprite = IconsConfig.Instance.GetByType(data.Item.IconType);
    }
}

