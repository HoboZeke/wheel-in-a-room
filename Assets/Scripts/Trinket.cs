using System;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Trinket : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    [SerializeField] TrinketProfile trinketProfile;

    public Mesh TrinketMesh { get { return mesh; } private set { mesh = value; } }
    public Material TrinketMaterial { get { return material; } private set { material = value; } }

    public string TrinketName { get { return trinketProfile.TrinketName; } private set { }  }
    public string TrinketDescription { get { return trinketProfile.TrinketDescription; } private set { } }

    public void Setup(TrinketProfile profile)
    {
        trinketProfile = profile;
        mesh = trinketProfile.TrinketMesh;
        material = trinketProfile.TrinketMaterial;
        SetupTrinketListeners();

        if (profile.RewardsOnPickup)
        {
            TriggerReward();
        }
    }

    public void SetupTrinketListeners(bool sub = true)
    {
        switch (trinketProfile.TrinketListen)
        {
            case TrinketProfile.TrinketListenEvent.TwoInARow:
                if(sub) TrinketManager.main.OnTwoInARow += ListenerTriggered;
                else TrinketManager.main.OnTwoInARow -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ThreeInARow:
                if(sub) TrinketManager.main.OnThreeInARow += ListenerTriggered;
                else TrinketManager.main.OnThreeInARow -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.FourInARow:
                if(sub) TrinketManager.main.OnFourInARow += ListenerTriggered;
                else TrinketManager.main.OnFourInARow -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourPresentInSpin:
                if(sub) TrinketManager.main.OnColourPresentInSpin += ListenerTriggered;
                else TrinketManager.main.OnColourPresentInSpin -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAbsentInSpin:
                if(sub) TrinketManager.main.OnColourAbsentInSpin += ListenerTriggered;
                else TrinketManager.main.OnColourAbsentInSpin -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAppearsTwice:
                if(sub) TrinketManager.main.OnColourAppearedTwiceInSpin += ListenerTriggered;
                else TrinketManager.main.OnColourAppearedTwiceInSpin -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAppearsThreeTimes:
                if(sub) TrinketManager.main.OnColourAppearedThreeTimesInSpin += ListenerTriggered;
                else TrinketManager.main.OnColourAppearedThreeTimesInSpin -= ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.EndOfSpin:
                if (sub) TrinketManager.main.OnSpinComplete += ListenerTriggered;
                else TrinketManager.main.OnSpinComplete -= ListenerTriggered;
                break;
        }
    }

    public void ListenerTriggered(object sender, TrinketEventArgs eventArgs)
    {
        if (trinketProfile.ListenerCaresAboutColour)
        {
            if(eventArgs.segmentColour != trinketProfile.ListenColour) { return; }
        }

        if (trinketProfile.RewardCondition != TrinketProfile.TrinketRewardCondition.None)
        {
            switch (trinketProfile.RewardCondition)
            {
                case TrinketProfile.TrinketRewardCondition.SingleArrow:
                    if(Wheel.main.ArrowCount() != 1) { return; }
                    break;
            }
        }

        TriggerReward(eventArgs);
    }

    void TriggerReward(TrinketEventArgs eventArgs = null)
    {
        switch (trinketProfile.TrinketReward)
        {
            case TrinketProfile.TrinketRewardType.IncreaseRewardValue:
                if (trinketProfile.RewardColour == WheelSegment.SegmentColour.None)
                {
                    Archive.main.RewardProfileForSegmentColour(eventArgs.segmentColour).IncreaseRewardType(trinketProfile.RewardType, trinketProfile.RewardStrength);
                }
                else
                {
                    Archive.main.RewardProfileForSegmentColour(trinketProfile.RewardColour).IncreaseRewardType(trinketProfile.RewardType, trinketProfile.RewardStrength);
                }
                break;
            case TrinketProfile.TrinketRewardType.IncreaseOtherRewardValue:
                foreach (WheelSegment.SegmentColour c in Enum.GetValues(typeof(WheelSegment.SegmentColour)))
                {
                    if (c != WheelSegment.SegmentColour.None && c != trinketProfile.RewardColour)
                    {
                        Archive.main.RewardProfileForSegmentColour(c).IncreaseRewardType(trinketProfile.RewardType, trinketProfile.RewardStrength);
                    }
                }
                break;
            case TrinketProfile.TrinketRewardType.GainReward:
                switch (trinketProfile.RewardType)
                {
                    case RewardProfile.RewardType.Fuel:
                        RewardShoot.main.SpawnFuelReward(trinketProfile.RewardStrength);
                        break;
                    case RewardProfile.RewardType.Coins:
                        CoinSpawner.main.SpawnCoins(trinketProfile.RewardStrength);
                        break;
                    case RewardProfile.RewardType.All:
                        RewardShoot.main.SpawnFuelReward(trinketProfile.RewardStrength);
                        CoinSpawner.main.SpawnCoins(trinketProfile.RewardStrength);
                        break;
                    case RewardProfile.RewardType.Spin:
                        ProgressTracker.main.AddSpins(trinketProfile.RewardStrength);
                        break;
                }
                break;
            case TrinketProfile.TrinketRewardType.RepeatPreviousSpinRewards:
                SpinLog log = RunLogger.main.ActiveRunLog().LastSpinInList();
                if(log.SpinRewardedCoinCount() > 0) { CoinSpawner.main.SpawnCoins(log.SpinRewardedCoinCount() * trinketProfile.RewardStrength); }
                if(log.SpinRewardedFuelCount() > 0) { RewardShoot.main.SpawnFuelReward(log.SpinRewardedFuelCount() * trinketProfile.RewardStrength); }
                break;

        }
    }

    public void ClearListeners()
    {
        SetupTrinketListeners(false);
    }

    private void OnDestroy()
    {
        ClearListeners();
    }
}
