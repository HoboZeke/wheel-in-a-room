using System.Collections.Generic;
using UnityEngine;

public class RewardBoard : MonoBehaviour
{
    public static RewardBoard main;
    
    [SerializeField] GameObject colourProfileUIPrefab;
    [SerializeField] Transform holder;
    List<ColourRewardProfileUI> colourProfiles = new List<ColourRewardProfileUI>();

    private void Awake()
    {
        main = this;
    }

    public void SegmentUpdate(WheelSegment.SegmentColour updatingColour)
    {
        ColourRewardProfileUI existingProfile = GetProfileForColour(updatingColour);

        if(existingProfile == null) 
        {
            GameObject go = Instantiate(colourProfileUIPrefab);
            go.transform.SetParent(holder);
            go.transform.localPosition = new Vector3(go.transform.localPosition.x, go.transform.localPosition.y, 0f);
            go.transform.localScale = Vector3.one;

            ColourRewardProfileUI profileUI = go.GetComponent<ColourRewardProfileUI>();
            colourProfiles.Add(profileUI);
            profileUI.Setup(updatingColour);
        }
        else
        {
            existingProfile.UpdateInfo();
        }
    }

    bool HasProfileForColour(WheelSegment.SegmentColour colour)
    {
        foreach (ColourRewardProfileUI p in colourProfiles)
        {
            if(p.Colour == colour) return true;
        }

        return false;
    }

    ColourRewardProfileUI GetProfileForColour(WheelSegment.SegmentColour colour)
    {
        foreach (ColourRewardProfileUI p in colourProfiles)
        {
            if (p.Colour == colour) return p;
        }

        return null;
    }
}
