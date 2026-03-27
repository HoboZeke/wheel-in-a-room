using System;
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
    }

    public void SetupTrinketListeners()
    {
        switch (trinketProfile.TrinketListen)
        {
            case TrinketProfile.TrinketListenEvent.TwoInARow:
                TrinketManager.main.OnTwoInARow += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ThreeInARow:
                TrinketManager.main.OnThreeInARow += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.FourInARow:
                TrinketManager.main.OnFourInARow += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourPresentInSpin:
                TrinketManager.main.OnColourPresentInSpin += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAbsentInSpin:
                TrinketManager.main.OnColourAbsentInSpin += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAppearsTwice:
                TrinketManager.main.OnColourAppearedTwiceInSpin += ListenerTriggered;
                break;
            case TrinketProfile.TrinketListenEvent.ColourAppearsThreeTimes:
                TrinketManager.main.OnColourAppearedThreeTimesInSpin += ListenerTriggered;
                break;
        }
    }

    public void ListenerTriggered(object sender, TrinketEventArgs eventArgs)
    {
        if (trinketProfile.ListenerCaresAboutColour)
        {
            if(eventArgs.segmentColour != trinketProfile.ListenColour) { return; }
        }

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
                }
                break;

        }
    }
}
