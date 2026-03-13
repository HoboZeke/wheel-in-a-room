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
                RewardShoot.main.SpawnFuelReward(amount);
                break;
            case RewardType.Coins:
                CoinSpawner.main.SpawnCoins(amount);
                break;
        }
    }
}
