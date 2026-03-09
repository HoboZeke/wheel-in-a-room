using UnityEngine;

public class RewardProfile : MonoBehaviour
{
    public enum RewardType { Fuel }
    [SerializeField] RewardType rewardType;
    [SerializeField] int amount;

    public virtual void ProcessReward(WheelSegment fromSegment) 
    {
        switch (rewardType)
        {
            case RewardType.Fuel:
                RewardShoot.main.SpawnFuelReward(amount);
                break;
        }
    }
}
