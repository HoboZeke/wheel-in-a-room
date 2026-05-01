using UnityEngine;

[CreateAssetMenu(fileName = "TrinketProfile", menuName = "ScriptableObjects/TrinketProfile")]
public class TrinketProfile : ScriptableObject
{
    [SerializeField] string trinketName;
    [SerializeField] string trinketDescription;
    [SerializeField] int trinketIndex;
    [SerializeField] bool rewardsOnPickup;

    [Header("Visuals")]
    [SerializeField] Mesh trinketMesh;
    [SerializeField] Material trinketMaterial;
    [SerializeField] Vector3 trinketPositionOffset;
    [SerializeField] Vector3 trinketObjectScale;
    [SerializeField] Vector3 trinketObjectEuler;

    [Header("Listening")]
    [SerializeField] TrinketListenEvent trinketListenEvent;
    [SerializeField] bool listenerCaresAboutColour;
    [SerializeField] WheelSegment.SegmentColour listenColour;
    [SerializeField] bool listenerCaresAboutWheelSize;
    [SerializeField] Vector2Int wheelSize;

    [Header("Rewards")]
    [SerializeField] TrinketRewardType trinketRewardType;
    [SerializeField] TrinketRewardCondition trinketRewardCondition;
    [SerializeField] WheelSegment.SegmentColour rewardColour;
    [SerializeField] RewardProfile.RewardType rewardType;
    [SerializeField] int rewardStrength;

    public string TrinketName { get { return trinketName; } private set { trinketName = value; } }
    public string TrinketDescription { get { return trinketDescription; } private set { trinketDescription = value; } }
    public int TrinketIndex { get { return trinketIndex; } private set { trinketIndex = value; } }
    public void SetTrinketIndex(int index) {  TrinketIndex = index; }

    public Mesh TrinketMesh { get { return trinketMesh; } private set { trinketMesh = value; } }
    public Material TrinketMaterial { get { return trinketMaterial; } private set { trinketMaterial = value; } }
    public Vector3 TrinketObjectOffset { get { return trinketPositionOffset; } private set { trinketPositionOffset = value; } }
    public Vector3 TrinketObjectScale { get { return trinketObjectScale; } private set { trinketObjectScale = value; } }
    public Vector3 TrinketObjectEuler { get { return trinketObjectEuler; } private set { trinketObjectEuler = value; } }

    public bool RewardsOnPickup { get { return rewardsOnPickup; } private set { rewardsOnPickup = value; } }
    public enum TrinketListenEvent { None, TwoInARow, ThreeInARow, FourInARow, ColourPresentInSpin, ColourAbsentInSpin, ColourAppearsTwice, ColourAppearsThreeTimes,
    ColourScores, EndOfSpin };
    public TrinketListenEvent TrinketListen { get { return trinketListenEvent; } private set { trinketListenEvent = value; } }

    public bool ListenerCaresAboutColour { get { return listenerCaresAboutColour; } private set { listenerCaresAboutColour = value; } }
    public WheelSegment.SegmentColour ListenColour { get { return listenColour; } private set { listenColour = value; } }
    public bool ListenerCaresAboutWheelSize { get { return listenerCaresAboutWheelSize; } private set { listenerCaresAboutWheelSize = value; } }
    public int MaxWheelSize { get { return wheelSize.y; } private set { wheelSize.y = value; } }
    public int MinWheelSize { get { return wheelSize.x; } private set { wheelSize.x = value; } }
    public Vector2Int WheelSizeLimits { get { return wheelSize; } private set { wheelSize = value; } }

    public enum TrinketRewardType { None, IncreaseRewardValue, IncreaseOtherRewardValue, GainReward, RepeatPreviousSpinRewards };
    public TrinketRewardType TrinketReward { get { return trinketRewardType; } private set { trinketRewardType = value; } }

    public enum TrinketRewardCondition { None, SingleArrow };
    public TrinketRewardCondition RewardCondition { get { return trinketRewardCondition; } private set { trinketRewardCondition = value; } }

    public WheelSegment.SegmentColour RewardColour { get { return rewardColour; } private set { rewardColour = value; } }
    public RewardProfile.RewardType RewardType { get { return rewardType; } private set { rewardType = value; } }
    public int RewardStrength { get { return rewardStrength; } private set { rewardStrength = value; } }
}
