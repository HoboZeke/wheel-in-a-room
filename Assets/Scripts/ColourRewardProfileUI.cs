using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ColourRewardProfileUI : MonoBehaviour
{
    public WheelSegment.SegmentColour Colour;

    [SerializeField] Image colourSquare;
    [SerializeField] TextMeshProUGUI rewardText;

    public void Setup(WheelSegment.SegmentColour colour)
    {
        Colour = colour;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        colourSquare.color = Archive.main.ColourForColourProfile(Colour);

        RewardProfile p = Archive.main.RewardProfileForSegmentColour(Colour);

        switch (p.RewardTypeEnum())
        {
            case RewardProfile.RewardType.Fuel:
                rewardText.text = "Fuel x" + p.RewardAmount();
                break;
            case RewardProfile.RewardType.Coins:
                rewardText.text = "Coin x" + p.RewardAmount();
                break;
        }
    }
}
