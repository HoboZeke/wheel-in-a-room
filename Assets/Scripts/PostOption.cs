using UnityEngine;


[CreateAssetMenu(fileName = "PostOption", menuName = "ScriptableObjects/PostOption")]
public class PostOption : ScriptableObject
{
    [SerializeField] string postName;
    [SerializeField] string postDescription;
    public enum PostReward { GainResource, AdjustColourRewards, AdjustColourSize, SootTradeOff, AdjustLargestColourSize, AdjustSmallestColourSize }
    [SerializeField] PostReward postReward;


    [Header("Opitionals")]
    [SerializeField] int strength;
    [SerializeField] RewardProfile.RewardType resourceType;
    [SerializeField] WheelSegment.SegmentColour[] positivelyEffectsColour, negativelyEffectsColour;

    public string PostName {  get { return postName; } set { postName = value; } }
    public string PostDescription { get { return postDescription; } set { postDescription = value; } }
    public int Strength { get { return strength; } set { strength = value; } } 
    public PostReward Reward { get { return postReward; } set { postReward = value; } }
    public RewardProfile.RewardType ResourceType { get { return resourceType; } set { resourceType = value; } }
    public WheelSegment.SegmentColour[] PositiveSegmentColour { get { return positivelyEffectsColour; } set { positivelyEffectsColour = value; } }
    public WheelSegment.SegmentColour[] NegativeSegmentColour { get { return negativelyEffectsColour; } set { negativelyEffectsColour = value; } }
}
