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
            case TrinketProfile.TrinketListenEvent.ThreeInARow:
                TrinketManager.main.OnThreeInARow += ListenerTriggered;
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
        }
    }
}
