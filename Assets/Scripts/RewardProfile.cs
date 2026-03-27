using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "RewardProfile", menuName = "ScriptableObjects/RewardProfile")]
public class RewardProfile : ScriptableObject
{
    public enum RewardType { Fuel, Coins, All }
    [SerializeField] RewardType[] baseRewardTypes;
    [SerializeField] int[] baseAmounts;

    [SerializeField] List<RewardType> rewardTypes = new List<RewardType>();
    [SerializeField] List<int> amounts = new List<int>();

    public EventHandler<EventArgs> OnValueChanged;

    public void Setup()
    {
        rewardTypes = new List<RewardType>(baseRewardTypes);
        amounts = new List<int>(baseAmounts);
    }

    public virtual void ProcessReward(WheelSegment fromSegment) 
    {
        for (int i = 0; i < rewardTypes.Count; i++)
        {
            RewardType rewardType = rewardTypes[i];
            int amount = amounts[i];
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
    }

    public RewardType RewardTypeEnum() { return rewardTypes[0]; }
    public RewardType RewardTypeEnum(int i) {  return rewardTypes[i]; }
    public RewardType[] RewardTypes() { return rewardTypes.ToArray(); }
    public int RewardAmount() { return amounts[0]; }
    public int RewardAmount(int i) { return amounts[i]; }

    public void IncreaseAllRewards(int increase)
    {
        for (int i = 0; i < rewardTypes.Count; i++)
        {
            amounts[i] += increase;
        }

        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseAllRewards(int decrease)
    {
        for (int i = rewardTypes.Count-1; i>= 0; i--)
        {
            amounts[i] -= decrease;
            if(amounts[i] <= 0)
            {
                rewardTypes.RemoveAt(i);
                amounts.RemoveAt(i);
            }
        }

        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public void IncreaseRewardType(RewardType type, int increase)
    {
        if(type == RewardType.All) { IncreaseAllRewards(increase); return; }

        for(int i = 0; i < rewardTypes.Count; i++)
        {
            if(rewardTypes[i] == type) { amounts[i] += increase; return; }
        }

        rewardTypes.Add(type);
        amounts.Add(increase);
        OnValueChanged?.Invoke(this, EventArgs.Empty);
    }

    public void DecreaseRewardType(RewardType type, int decrease)
    {
        if (type == RewardType.All) { DecreaseAllRewards(decrease); return; }

        for (int i = 0; i < rewardTypes.Count; i++)
        {
            if (rewardTypes[i] == type) 
            { 
                amounts[i] -= decrease;
                if(amounts[i] <= 0)
                {
                    amounts.RemoveAt(i);
                    rewardTypes.RemoveAt(i);
                }
                OnValueChanged?.Invoke(this, EventArgs.Empty);

                return; 
            }
        }

    }

    public int CoinRewardAmount()
    {
        for(int i = 0; i < rewardTypes.Count; i++) 
        {
            if (rewardTypes[i] == RewardType.Coins) return amounts[i];
        }
        
        return 0;
    }

    public int FuelRewardAmount()
    {
        for (int i = 0; i < rewardTypes.Count; i++)
        {
            if (rewardTypes[i] == RewardType.Fuel) return amounts[i];
        }

        return 0;
    }
}
