
using UnityEngine;

[System.Serializable]
public class RewardItemData : MonoBehaviour
{
    [field: SerializeField] public RewardType Type { get; private set; }

    [field: SerializeField] public int Amount { get; private set; }

    public void SetAmount(int value)
        => Amount = value;
}

public enum RewardType
{
    CursedGold = 0,
    Skull = 1,
    EndlessLifes = 2,
    InventoryItem = 3,
    FreeSpin = 4,
}