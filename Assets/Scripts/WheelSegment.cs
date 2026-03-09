using UnityEngine;
using UnityEngine.UI;

public class WheelSegment : MonoBehaviour
{
    [SerializeField] RectTransform rt;
    [SerializeField] Color color;
    [SerializeField] Image image;
    [SerializeField] int size;
    [SerializeField] RewardProfile reward;

    public void Setup(int s, Color c, RewardProfile reward)
    {
        this.color = c;
        this.reward = reward;
        size = s;
        rt.anchoredPosition = Vector3.zero;
        rt.offsetMax = Vector3.zero;
        rt.offsetMin = Vector3.zero;
        

        UpdateVisual();
    }

    public int SegmentSize() { return size; }

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


}
