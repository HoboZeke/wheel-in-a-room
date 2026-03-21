using UnityEngine;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour
{
    public enum SegmentColour { White, Green, Red, None};
    [SerializeField] RectTransform rt;
    [SerializeField] Color color;
    [SerializeField] SegmentColour segmentColour;
    [SerializeField] Image image;
    [SerializeField] int size;
    [SerializeField] RewardProfile reward;

    public void Setup(int s, Color c, SegmentColour sColour)
    {
        this.color = c;
        segmentColour = sColour;
        this.reward = Archive.main.RewardProfileForSegmentColour(segmentColour);
        size = s;
        rt.anchoredPosition = Vector3.zero;
        rt.offsetMax = Vector3.zero;
        rt.offsetMin = Vector3.zero;


        UpdateVisual();
    }

    public int SegmentSize() { return size; }
    public void AdjustSegmentSize(int amount)
    {
        size += amount;
        if(size < -1) {  size = 0; }
    }

    public float SegmentActualSize() { return (float)size / Wheel.main.WheelSize(); }

    public float AngleOnWheel() {  return SegmentActualSize() * 360; }

    public void UpdateVisual()
    {
        image.color = color;
        image.fillAmount = SegmentActualSize();
    }

    public void GainReward()
    {
        reward.ProcessReward(this);
    }

    public int RewardCoins() { return reward.CoinRewardAmount(); }
    public int RewardFuel() { return reward.FuelRewardAmount(); }

    public SegmentColour SegColour() { return segmentColour; }

}
