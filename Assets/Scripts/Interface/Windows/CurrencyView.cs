using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyView : MonoBehaviour
{
    [SerializeField] private CurrencyType type;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private Image icon;

    private void OnEnable()
    {
        if(type == CurrencyType.Soft)
        {
            icon.sprite = IconsConfig.Instance.GetByType(IconType.SoftCurrency);
            CurrencyService.OnSoftChanged += UpdateAmount;
            UpdateAmount(CurrencyService.GetCurrency(type));
        }
        else if(type == CurrencyType.Hard)
        {
            icon.sprite = IconsConfig.Instance.GetByType(IconType.HardCurrency);
            CurrencyService.OnHardChanged += UpdateAmount;
            UpdateAmount(CurrencyService.GetCurrency(type));
        }
    }

    private void OnDisable()
    {
        if(type == CurrencyType.Soft)
            CurrencyService.OnSoftChanged -= UpdateAmount;
        else if(type == CurrencyType.Hard)
            CurrencyService.OnHardChanged -= UpdateAmount;
    }

    private void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}