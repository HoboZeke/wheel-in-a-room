using System;
using UnityEngine;

public class Trinket : MonoBehaviour
{
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;

    [SerializeField] TrinketProfile trinketProfile;

    public Mesh TrinketMesh { get { return mesh; } private set { mesh = value; } }
    public Material TrinketMaterial { get { return material; } private set { material = value; } }

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
                    if(c != WheelSegment.SegmentColour.None && c != trinketProfile.RewardColour)
                    {
                        Archive.main.RewardProfileForSegmentColour(c).IncreaseRewardType(trinketProfile.RewardType, trinketProfile.RewardStrength);
                    }
                }
                break;
        }
    }
}
