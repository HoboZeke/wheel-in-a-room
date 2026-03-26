using System;
using System.Collections.Generic;
using UnityEngine;

public class RunLogger : MonoBehaviour
{
    public static RunLogger main;

    List<RunLog> logList = new List<RunLog>();

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        logList.Add(new RunLog() { RunNumber = 1 });
    }

    RunLog ActiveRunLog() { return logList[logList.Count - 1]; }
    SpinLog ActiveSpinLog() { return ActiveRunLog().LastSpinInList(); }

    public void OnSpin()
    {
        ActiveRunLog().AddNewSpin();
        TrinketManager.main.WheelSpun();
    }

    public void OnReward(WheelSegment.SegmentColour c, int coin, int fuel)
    {
        ActiveSpinLog().AddSpinRewardData(c, coin, fuel);
        TrinketManager.main.RewardGained(c);
    }

    SpinLog RecentSpin(int spinIndex)
    {
        if(spinIndex == 0) { return ActiveSpinLog(); }

        RunLog run = ActiveRunLog();

        if(run.SpinCount() < spinIndex)
        {
            return run.HistoricSpin(spinIndex);
        }
        else
        {
            return null;
        }
    }

    public void CheckRewardTrends()
    {
        int consecutiveWhiteReward = 0;
        bool whiteRewardsTallied = false;
        int consecutiveRedReward = 0;
        bool redRewardsTallied = false;
        int consecutiveGreenReward = 0;
        bool greenRewardsTallied = false;
        bool checking = true;

        int checkingSpin = 0;

        while (checking)
        {
            SpinLog activeSpin = RecentSpin(checkingSpin);
            if(activeSpin == null) { checking = false; break; }

            if (!whiteRewardsTallied)
            {
                if (activeSpin.SpinRewardedColour(WheelSegment.SegmentColour.White))
                {
                    consecutiveWhiteReward++;
                }
                else
                {
                    whiteRewardsTallied = true;
                }
            }

            if (!redRewardsTallied)
            {
                if (activeSpin.SpinRewardedColour(WheelSegment.SegmentColour.Red))
                {
                    consecutiveRedReward++;
                }
                else
                {
                    redRewardsTallied = true;
                }
            }

            if (!greenRewardsTallied)
            {
                if (activeSpin.SpinRewardedColour(WheelSegment.SegmentColour.Green))
                {
                    consecutiveGreenReward++;
                }
                else
                {
                    greenRewardsTallied = true;
                }
            }

            checkingSpin--;
        }

        CheckConsecutiveRewards(WheelSegment.SegmentColour.White, consecutiveWhiteReward);
        CheckConsecutiveRewards(WheelSegment.SegmentColour.Green, consecutiveGreenReward);
        CheckConsecutiveRewards(WheelSegment.SegmentColour.Red, consecutiveRedReward);

        CheckPresentAndAbsentColours(RecentSpin(0));

    }

    void CheckConsecutiveRewards(WheelSegment.SegmentColour c, int consec)
    {
        int pairs = Mathf.FloorToInt(consec / 2);
        float excessPair = consec % 2;
        int triples = Mathf.FloorToInt(consec / 3);
        float excessTriples = consec % 3;
        int quads = Mathf.FloorToInt(consec / 4);
        float excessQuads = consec % 4;

        if(pairs > 0 && excessPair == 0)
        {
            TrinketManager.main.TwoRewardsOfTheSameColourInARow(c);
        }

        if (triples > 0 && excessTriples == 0)
        {
            TrinketManager.main.ThreeRewardsOfTheSameColourInARow(c);
        }

        if(quads > 0 && excessQuads == 0)
        {
            TrinketManager.main.FourRewardsOfTheSameColourInARow(c);
        }
    }

    void CheckPresentAndAbsentColours(SpinLog log)
    {
        foreach (WheelSegment.SegmentColour c in Enum.GetValues(typeof(WheelSegment.SegmentColour)))
        {
            if(c != WheelSegment.SegmentColour.None)
            {
                if (log.SpinRewardedColour(c)) { TrinketManager.main.ColourSegmentPresent(c); }
                else { TrinketManager.main.ColourSegmentAbsent(c); }
            }
        }
    }
}

public class RunLog
{
    public int RunNumber;
    public List<SpinLog> spins = new List<SpinLog>();

    public SpinLog LastSpinInList() { return spins[spins.Count - 1]; }
    public int SpinCount() { return spins.Count; }
    public SpinLog HistoricSpin(int spinIndex) {  return spins[spins.Count - spinIndex - 1]; }
    public void AddNewSpin() 
    { 
        spins.Add(new SpinLog() { SpinNumber = spins.Count + 1 });
    }
}

public class SpinLog
{
    public int SpinNumber;
    public SpinRewardLog spinRewardLog;

    public void AddSpinRewardData(WheelSegment.SegmentColour c, int coin, int fuel)
    {
        if(spinRewardLog == null) { spinRewardLog = new SpinRewardLog(c, coin, fuel); }
        else { spinRewardLog.AddSpinRewardInfo(c, coin, fuel); }
    }

    public bool SpinRewardedColour(WheelSegment.SegmentColour c)
    {
        foreach(WheelSegment.SegmentColour sC in spinRewardLog.SegmentColours)
        {
            if(c == sC) return true;
        }

        return false;
    }
}

public class SpinRewardLog
{
    public List<WheelSegment.SegmentColour> SegmentColours = new List<WheelSegment.SegmentColour>();
    public List<int> CoinsGained = new List<int>();
    public List<int> FuelGained = new List<int>();

    public SpinRewardLog(WheelSegment.SegmentColour c, int coin, int fuel)
    {
        SegmentColours.Add(c);
        CoinsGained.Add(coin);
        FuelGained.Add(fuel);
    }

    public SpinRewardLog(WheelSegment.SegmentColour[] c, int[] coins, int[] fuel)
    {
        SegmentColours.AddRange(c);
        CoinsGained.AddRange(coins);
        FuelGained.AddRange(fuel);
    }

    public void AddSpinRewardInfo(WheelSegment.SegmentColour c, int coin, int fuel)
    {
        SegmentColours.Add(c);
        CoinsGained.Add(coin);
        FuelGained.Add(fuel);
    }
}
