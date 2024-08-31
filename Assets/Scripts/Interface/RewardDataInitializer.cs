
using System;
using TMPro;
using UnityEngine;

public class RewardDataInitializer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI amount;

    private void Start()
    {
        Initialize();
    }

    public void Initialize()
    {
        var data = gameObject.GetComponent<RewardItemData>();
        itemName.text = data.Type.ToString();
        amount.text = data.Amount.ToString();
    }
}

