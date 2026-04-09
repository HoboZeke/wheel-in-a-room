using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniWheelSegment : MonoBehaviour
{
    public enum MiniSegmentColour { White, Green, Red, None, Soot, Unique };
    [SerializeField] RectTransform rt;
    [SerializeField] Color color;
    [SerializeField] MiniSegmentColour segmentColour;
    [SerializeField] Image image;
    [SerializeField] int size;
    [SerializeField] RewardProfile reward;

    [Header("Label")]
    [SerializeField] Transform labelAnchor;
    [SerializeField] RectTransform labelAnchorRect, labelRect;
    [SerializeField] TextMeshProUGUI labelText;

    public void Setup(int s, MiniSegmentColour sColour)
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
        if (size < -1) { size = 0; }

        if (size == 0)
        {
            DeleteSegment();
        }
    }

    void DeleteSegment()
    {
        Wheel.main.RemoveMiniSegment(this);
        reward.OnValueChanged -= RewardProfileUpdated;
        Destroy(gameObject);
    }

    public float SegmentActualSize() { return (float)size / Wheel.main.MiniWheelSize(); }

    public float AngleOnWheel() { return SegmentActualSize() * 360; }

    public void UpdateVisual()
    {
        image.color = color;
        image.fillAmount = SegmentActualSize();
        UpdateLabel();
    }

    public int AdjustRewardStrength(int strength)
    {
        strength = Mathf.RoundToInt(strength * reward.MiniWheelMulti());
        return strength;
    }

    public int RewardCoins() { return reward.CoinRewardAmount(); }
    public int RewardFuel() { return reward.FuelRewardAmount(); }

    public MiniSegmentColour SegColour() { return segmentColour; }

    public void RewardProfileUpdated(object sender, EventArgs eventArgs)
    {
        UpdateLabel();
    }

    public void UpdateLabel()
    {
        labelText.text = LabelText();

        labelAnchor.localEulerAngles = Vector3.forward * (image.fillAmount * 180f);
        float sizeMod = Mathf.Clamp01(image.fillAmount * 2f);
        labelRect.sizeDelta = new Vector2(labelAnchorRect.rect.width / 2f, labelAnchorRect.rect.height / 2f) * sizeMod;

        if (image.fillAmount > 0.05f)
        {
            labelRect.localEulerAngles = Vector3.zero;
            labelRect.sizeDelta = new Vector2(labelRect.sizeDelta.y, labelRect.sizeDelta.x);
            labelRect.anchoredPosition = new Vector2(0f, -labelAnchorRect.rect.height / 7f);
        }
        else
        {
            labelRect.localEulerAngles = new Vector3(0f, 0f, 90f);
            labelRect.sizeDelta = new Vector2(labelRect.sizeDelta.y, labelRect.sizeDelta.x);
            labelRect.anchoredPosition = new Vector2(0f, -labelAnchorRect.rect.height / 7f);
        }
    }

    string LabelText()
    {
        string s = "";
        
        if(reward.MiniWheelRewardType() == RewardProfile.RewardType.Multiplier)
        {
            s += "x" + reward.MiniWheelMulti().ToString();
        }

        return s;
    }
}
