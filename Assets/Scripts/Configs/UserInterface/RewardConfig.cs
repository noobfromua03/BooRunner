using RandomGeneratorWithWeight;
using System.Collections.Generic;
using UnityEngine;

public class RewardConfig : AbstractConfig<RewardConfig>
{
    [Header("Wheel")]
    [SerializeField] private List<ItemForRandom<RewardItemData>> wheelRewards;

    [Header("Level")]
    [SerializeField] private ItemType levelRewardItemType;
    [SerializeField] private float goldLoafMultiplier;
    [SerializeField] private float defaultMultiplier;
    [SerializeField] private int minReward;

    public RewardItemData GetSoftLevelReward(bool isGoldLoaf, int score)
    {
        int value = (int)((isGoldLoaf ? score * goldLoafMultiplier : score) * defaultMultiplier);
        value = score != 0 ? Mathf.Max(value, minReward) : 0;
        return new RewardItemData(levelRewardItemType, value);
    }

    public List<RewardItemData> GetAllWheelRewards()
    {
        var rewardItems = new List<RewardItemData>();
        foreach (var item in wheelRewards)
            rewardItems.Add(item.GetItem());
        return rewardItems;
    }

    public (RewardItemData, int) GetWheelReward()
    {
        return GetItemWithWeight.GetItemWithIndex(wheelRewards);
    }
}

[System.Serializable]
public class RewardItemData
{
    [field: SerializeField] public ItemType Type { get; private set; }
    [field: SerializeField] public int Amount { get; private set; }
    public IInventoryItem Item => ItemBuilder.GetInventoryItem(Type);
    public RewardItemData() { }
    public RewardItemData(ItemType itemType, int amount) 
    {
        Type = itemType;
        Amount = amount;
    }
}