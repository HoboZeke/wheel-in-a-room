using UnityEngine;

[CreateAssetMenu(fileName = "TrinketProfile", menuName = "ScriptableObjects/TrinketProfile")]
public class TrinketProfile : ScriptableObject
{
    [SerializeField] string trinketName;
    [SerializeField] string trinketDescription;
    [SerializeField] int trinketIndex;
    [SerializeField] Mesh trinketMesh;
    [SerializeField] Material trinketMaterial;
    [Header("Listening")]
    [SerializeField] TrinketListenEvent trinketListenEvent;
    [SerializeField] bool listenerCaresAboutColour;
    [SerializeField] WheelSegment.SegmentColour listenColour;
    [Header("Rewards")]
    [SerializeField] TrinketRewardType trinketRewardType;
    [SerializeField] WheelSegment.SegmentColour rewardColour;
    [SerializeField] RewardProfile.RewardType rewardType;
    [SerializeField] int rewardStrength;

    public string TrinketName { get { return trinketName; } private set { trinketName = value; } }
    public string TrinketDescription { get { return trinketDescription; } private set { trinketDescription = value; } }
    public int TrinketIndex { get { return trinketIndex; } private set { trinketIndex = value; } }
    public void SetTrinketIndex(int index) {  TrinketIndex = index; }

    public Mesh TrinketMesh { get { return trinketMesh; } private set { trinketMesh = value; } }
    public Material TrinketMaterial { get { return trinketMaterial; } private set { trinketMaterial = value; } }

    public enum TrinketListenEvent { None, TwoInARow, ThreeInARow, FourInARow, ColourPresentInSpin, ColourAbsentInSpin, ColourAppearsTwice, ColourAppearsThreeTimes,
    ColourScores };
    public TrinketListenEvent TrinketListen { get { return trinketListenEvent; } private set { trinketListenEvent = value; } }

    public bool ListenerCaresAboutColour { get { return listenerCaresAboutColour; } private set { listenerCaresAboutColour = value; } }
    public WheelSegment.SegmentColour ListenColour { get { return listenColour; } private set { listenColour = value; } }

    public enum TrinketRewardType { None, IncreaseRewardValue, IncreaseOtherRewardValue, GainReward };
    public TrinketRewardType TrinketReward { get { return trinketRewardType; } private set { trinketRewardType = value; } }

    public WheelSegment.SegmentColour RewardColour { get { return rewardColour; } private set { rewardColour = value; } }
    public RewardProfile.RewardType RewardType { get { return rewardType; } private set { rewardType = value; } }
    public int RewardStrength { get { return rewardStrength; } private set { rewardStrength = value; } }
}
