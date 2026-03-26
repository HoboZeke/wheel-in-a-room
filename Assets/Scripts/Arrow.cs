using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] MeshFilter meshFilter;
    [SerializeField] MeshRenderer meshRenderer;

    [SerializeField] Wheel.WheelArrowClockPositions positionOnWheel;
    [SerializeField] ArrowProfile profile;

    public enum ArrowTag { None, Indestructable, Immovable, Brittle }

    int arrowHP;

    [ContextMenu("LoadFromProfile")]
    public void LoadExistingProfile() { LoadProfile(profile); }

    public void LoadProfile(ArrowProfile p)
    {
        profile = p;

        gameObject.name = p.ArrowName;

        meshFilter.mesh = p.ArrowMesh;
        meshRenderer.material = p.ArrowMaterial;

        if (p.IsBrittle)
        {
            arrowHP = p.StartHP;
        }
    }

    public void SegmentLandedUnderArrow(WheelSegment segment)
    {
        if (profile.RewardsSegmentUnderArrow)
        {
            TriggerRewardUnderArrow(segment);
        }

        if (profile.IsBrittle)
        {
            arrowHP -= 1;
        }
    }

    public void RewardCleanup()
    {
        if (arrowHP <= 0)
        {
            ArrowManager.main.RemoveArrow(this);
            Destroy(gameObject);
        }
    }

    void TriggerRewardUnderArrow(WheelSegment rewardSegment)
    {
        if (rewardSegment != null)
        {
            rewardSegment.GainReward();
            RunLogger.main.OnReward(rewardSegment.SegColour(), rewardSegment.RewardCoins(), rewardSegment.RewardFuel());
        }
        else
        {
            Debug.LogWarning("No reward segment found!!!");
        }
    }

    public string ArrowName() { return profile.ArrowName; }
    public string ArrowDescriptions() { return profile.ArrowDescription; }
    public string ArrowTypes() { return profile.ArrowTags; }
    public bool InteractsWithSegmentUnderArrow() { return profile.RewardsSegmentUnderArrow; }
    public void SetPositionOnWheel(Wheel.WheelArrowClockPositions pos) { positionOnWheel = pos; }
}
