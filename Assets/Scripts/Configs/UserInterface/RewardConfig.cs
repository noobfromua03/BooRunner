using RandomGeneratorWithWeight;
using System;
using System.Collections.Generic;
using System.Linq;
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

    public List<RewardItemData> UpdateAllWheelRewards()
    {
        var rewardItems = new List<RewardItemData>();
        var currentRewards = wheelRewards.OrderBy(i => UnityEngine.Random.value).Take(8).ToList();

        Progress.Inventory.spins.currentWheelRewardIndexes.Clear();

        foreach (var item in currentRewards)
        {
            rewardItems.Add(item.GetItem());
            int index = wheelRewards.IndexOf(item);
            Progress.Inventory.spins.currentWheelRewardIndexes.Add(index);
        }
        return rewardItems;
    }

    public List<RewardItemData> GetCurrentWheelReward()
    {
        var rewardItems = new List<RewardItemData>();

        Progress.Inventory.spins.currentWheelRewardIndexes = 
            Progress.Inventory.spins.currentWheelRewardIndexes.OrderBy(i => UnityEngine.Random.value).ToList();

        foreach (var index in Progress.Inventory.spins.currentWheelRewardIndexes)
            rewardItems.Add(wheelRewards[index].GetItem());

        return rewardItems;
    }

    public (RewardItemData, int) GetWheelReward()
    {
        var rewardItems = new List<ItemForRandom<RewardItemData>>();

        foreach (var index in Progress.Inventory.spins.currentWheelRewardIndexes)
            rewardItems.Add(wheelRewards[index]);

        return GetItemWithWeight.GetItemWithIndex(rewardItems);
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