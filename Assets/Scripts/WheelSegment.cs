using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour
{
    public enum SegmentColour { White, Green, Red, None, Soot, Jackpot};
    [SerializeField] RectTransform rt;
    [SerializeField] Color color;
    [SerializeField] SegmentColour segmentColour;
    [SerializeField] Image image;
    [SerializeField] int size;
    [SerializeField] RewardProfile reward;

    [Header("Label")]
    [SerializeField] Transform labelAnchor;
    [SerializeField] RectTransform labelAnchorRect, labelRect;
    [SerializeField] TextMeshProUGUI labelText;

    public void Setup(int s, SegmentColour sColour)
    {
        this.color = Archive.main.ColourForColourProfile(sColour);
        segmentColour = sColour;
        labelText.color = Archive.main.LabelColourForColourProfile(sColour);
        this.reward = Archive.main.RewardProfileForSegmentColour(segmentColour);
        size = s;
        rt.anchoredPosition = Vector3.zero;
        rt.offsetMax = Vector3.zero;
        rt.offsetMin = Vector3.zero;

        gameObject.name = segmentColour.ToString() + " Segment";

        reward.OnValueChanged += RewardProfileUpdated;

        UpdateVisual();
    }

    public int SegmentSize() { return size; }
    public void AdjustSegmentSize(int amount)
    {
        size += amount;
        if(size < -1) {  size = 0; }

        if(size == 0)
        {
            DeleteSegment();
        }
    }

    void DeleteSegment()
    {
        Wheel.main.RemoveSegment(this);
        reward.OnValueChanged -= RewardProfileUpdated;
        Destroy(gameObject);
    }

    public float SegmentActualSize() { return (float)size / Wheel.main.WheelSize(); }

    public float AngleOnWheel() {  return SegmentActualSize() * 360; }

    public void UpdateVisual()
    {
        image.color = color;
        image.fillAmount = SegmentActualSize();
        UpdateLabel();
    }

    public void GainReward()
    {
        reward.ProcessReward(this);
    }

    public void GainReward(int multi)
    {
        reward.ProcessReward(this, multi);
    }

    public void GainReward(MiniWheelSegment mulitSeg)
    {
        reward.ProcessReward(this, 1, mulitSeg);
    }

    public void GainReward(MiniWheelSegment mulitSeg, RewardProfile.RewardType selective, RewardProfile.RewardType convertive)
    {
        reward.ProcessReward(this, 1, mulitSeg, selective, convertive);
    }

    public int RewardCoins() { return reward.CoinRewardAmount(); }
    public int RewardFuel() { return reward.FuelRewardAmount(); }

    public SegmentColour SegColour() { return segmentColour; }

    public void RewardProfileUpdated(object sender, EventArgs eventArgs)
    {
        UpdateLabel();
    }

    public void UpdateLabel()
    {
        labelText.text = LabelText();

        labelAnchor.localEulerAngles = Vector3.forward * (image.fillAmount * 180f);
        float sizeMod = Mathf.Clamp01(image.fillAmount * 2f);
        labelRect.sizeDelta = new Vector2(labelAnchorRect.rect.width / 4f, labelAnchorRect.rect.height / 2f) * sizeMod;

        if(image.fillAmount > Wheel.main.SegmentLabelFlipThreshold())
        {
            labelRect.localEulerAngles = Vector3.zero;
            labelRect.sizeDelta = new Vector2(labelRect.sizeDelta.y, labelRect.sizeDelta.x);
            labelRect.anchoredPosition = new Vector2(0f, -labelAnchorRect.rect.height / Wheel.main.SegmentLabelPositionDivider());
        }
        else
        {
            labelRect.localEulerAngles = new Vector3(0f, 0f, 90f);
            labelRect.sizeDelta = new Vector2(labelRect.sizeDelta.y, labelRect.sizeDelta.x);
            labelRect.anchoredPosition = new Vector2(0f, -labelAnchorRect.rect.height / Wheel.main.SegmentLabelPositionDivider());
        }
    }

    string LabelText()
    {
        if (reward.IsEmpty()) { return "Nothing"; }

        string s = reward.RewardAmount() + " " + reward.RewardTypeEnum().ToString();
        RewardProfile.RewardType[] rewards = reward.RewardTypes();

        for(int i = 1; i < rewards.Length; i++)
        {
            s += "\n" + reward.RewardAmount(i) + " " + rewards[i].ToString();
        }

        return s;
    }

}
