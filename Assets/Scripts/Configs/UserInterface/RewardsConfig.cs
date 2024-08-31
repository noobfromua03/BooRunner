using RandomGeneratorWithWeight;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RewardsConfig : AbstractConfig<RewardsConfig>
{
    [field: SerializeField] public List<ItemForRandom<RewardItemData>> WheelRewards { get; private set; }

    public (RewardItemData, int) GetWheelReward()
    {
        return GetItemWithWeight.GetItemWithIndex(WheelRewards);
    }

}
