using System;
using UnityEngine;

public class TrinketManager : MonoBehaviour
{
    public static TrinketManager main;
    private void Awake() { main = this; }

    [SerializeField] TrinketCabinet trinketCabinet;
    [SerializeField] GameObject trinketPrefab;
    [SerializeField] TrinketProfile[] trinketProfiles;

    public EventHandler<TrinketEventArgs> OnWheelSpun;
    public EventHandler<TrinketEventArgs> OnRewardSegmentGained;
    public EventHandler<TrinketEventArgs> OnTwoInARow;
    public EventHandler<TrinketEventArgs> OnThreeInARow;
    public EventHandler<TrinketEventArgs> OnFourInARow;
    public EventHandler<TrinketEventArgs> OnColourPresentInSpin;
    public EventHandler<TrinketEventArgs> OnColourAbsentInSpin;
    public EventHandler<TrinketEventArgs> OnColourAppearedTwiceInSpin;
    public EventHandler<TrinketEventArgs> OnColourAppearedThreeTimesInSpin;
    public EventHandler<TrinketEventArgs> OnColourScored;

    [ContextMenu("UpdateTrinketProfileIndices")]
    void UpdateTrinketProfileIndices()
    {
        for (int i = 0; i < trinketProfiles.Length; i++)
        {
            trinketProfiles[i].SetTrinketIndex(i);
        }
    }

    public bool TrinketsFull() { return !trinketCabinet.HasSpace(); }

    public void CreateTrinket(int index)
    {
        if (trinketCabinet.HasSpace())
        {
            GameObject go = Instantiate(trinketPrefab);
            go.GetComponent<Trinket>().Setup(trinketProfiles[index]);
            go.GetComponent<TrinketObject>().SetupTrinket(go.GetComponent<Trinket>());

            trinketCabinet.AddTrinketToCabinet(go);
        }
    }

    public void WheelSpun()
    {
        OnWheelSpun?.Invoke(this, new TrinketEventArgs() { spinNumber = Wheel.main.SpinCount() });
    }

    public void RewardGained(WheelSegment.SegmentColour colour)
    {
        OnRewardSegmentGained?.Invoke(this, new TrinketEventArgs() { spinNumber = Wheel.main.SpinCount(), segmentColour = colour });
    }

    public void TwoRewardsOfTheSameColourInARow(WheelSegment.SegmentColour colour)
    {
        OnTwoInARow?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void ThreeRewardsOfTheSameColourInARow(WheelSegment.SegmentColour colour)
    {
        OnThreeInARow?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void FourRewardsOfTheSameColourInARow(WheelSegment.SegmentColour colour)
    {

        OnFourInARow?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void ColourSegmentPresent(WheelSegment.SegmentColour colour)
    {
        OnColourPresentInSpin?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void ColourSegmentAbsent(WheelSegment.SegmentColour colour)
    {
        OnColourAbsentInSpin?.Invoke(this, new TrinketEventArgs() {  segmentColour= colour });
    }

    public void ColourSegmentScoredTwice(WheelSegment.SegmentColour colour)
    {
        OnColourAppearedTwiceInSpin?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void ColourSegmentScoredThreeTimes(WheelSegment.SegmentColour colour)
    {
        OnColourAppearedTwiceInSpin?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }

    public void ColourScored(WheelSegment.SegmentColour colour)
    {
        OnColourScored?.Invoke(this, new TrinketEventArgs() { segmentColour = colour });
    }
}

public class TrinketEventArgs: EventArgs
{
    public WheelSegment.SegmentColour segmentColour;
    public int spinNumber;
}
