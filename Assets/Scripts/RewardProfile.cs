using UnityEngine;

[CreateAssetMenu(fileName = "RewardProfile", menuName = "ScriptableObjects/RewardProfile")]
public class RewardProfile : ScriptableObject
{
    public enum RewardType { Fuel, Coins }
    [SerializeField] RewardType rewardType;
    [SerializeField] int amount;

    public virtual void ProcessReward(WheelSegment fromSegment) 
    {
        switch (rewardType)
        {
            case RewardType.Fuel:
                Wheel.main.GainRewardResources(0, amount);
                break;
            case RewardType.Coins:
                Wheel.main.GainRewardResources(amount, 0);
                break;
        }
    }

    public RewardType RewardTypeEnum() { return rewardType; }
    public int RewardAmount() { return amount; }
}
