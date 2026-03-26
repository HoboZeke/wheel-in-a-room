using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Wheel : MonoBehaviour
{
    public static Wheel main;

    [SerializeField] Transform wheel;
    [SerializeField] ArrowSlot[] arrowSlots;
    [SerializeField] Transform segmentHolder;
    [SerializeField] Image highlightSegmentImage, overlayImage;
    [SerializeField] float highlightFillExcess;
    [SerializeField] GameObject wheelSegmentPrefab;
    public enum WheelState { Idle, Spinning, Reward }
    [SerializeField] WheelState currentState;

    public enum WheelArrowClockPositions { Twelve, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Eleven }
    List<WheelSegment> wheelSegments = new List<WheelSegment>();
    int spinCount;


    [Header("SpinSettings")]
    [SerializeField] float maxSpinSpeed;
    [SerializeField] float windUpDuration;
    [SerializeField] Vector2 spinDuration;

    [Header("RewardSettings")]
    [SerializeField] float timeBetweenRewards;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        ToggleTooltip(false);
        AddSegment(3, WheelSegment.SegmentColour.White);
        AddSegment(2, WheelSegment.SegmentColour.Red);
        AddSegment(1, WheelSegment.SegmentColour.Green);
        spinCount = 0;
    }

    private void Update()
    {
        if(focused && Input.GetKeyDown(KeyCode.Escape))
        {
            UnfocusWheel();
        }
    }

    public void AddSegment(int size, WheelSegment.SegmentColour sColour)
    {
        if(GetSegment(sColour) != null)
        {
            GetSegment(sColour).AdjustSegmentSize(size);
            AlignAllSegments();
            RewardBoard.main.SegmentUpdate(sColour);
            return;
        }

        GameObject obj = Instantiate(wheelSegmentPrefab);
        obj.transform.SetParent(segmentHolder);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        WheelSegment seg = obj.GetComponent<WheelSegment>();
        wheelSegments.Add(seg);
        seg.Setup(size, Archive.main.ColourForColourProfile(sColour), sColour);
        RewardBoard.main.SegmentUpdate(sColour);
        AlignAllSegments();
    }

    WheelSegment GetSegment(WheelSegment.SegmentColour sColour)
    {
        foreach (WheelSegment seg in wheelSegments)
        {
            if(seg.SegColour() == sColour) { return seg; }
        }
        return null;
    }

    void AlignAllSegments()
    {
        float zAngle = 0f;
        for (int i = 0; i < wheelSegments.Count; i++)
        {
            wheelSegments[i].transform.localEulerAngles = new Vector3(0f, 0f, zAngle);
            zAngle += wheelSegments[i].AngleOnWheel();
            wheelSegments[i].UpdateVisual();
        }
    }

    public void SpinTheWheel()
    {
        if (currentState != WheelState.Idle) { return; }

        spinCount++;
        ProgressTracker.main.UseSpin();
        RunLogger.main.OnSpin();
        ChangeState(WheelState.Spinning);
        StartCoroutine(AnimateWheelSpin(Random.Range(spinDuration.x, spinDuration.y)));

        
    }

    public int SpinCount() { return spinCount; }

    IEnumerator ProcessRewards()
    {
        ChangeState(WheelState.Reward);

        for (int i = 1; i < arrowSlots.Length; i++)
        {
            if (arrowSlots[i].HasArrow())
            {
                CheckRewardSegment(WheelArrowAngle(arrowSlots[i].ClockPosition()), arrowSlots[i].ArrowInSlot());
                yield return new WaitForSeconds(timeBetweenRewards);
                ClearRewardHighlight();
                arrowSlots[i].ArrowInSlot().RewardCleanup();
            }
        }

        //Finally cash in north arrow.
        CheckRewardSegment(TopOfWheelAngle(), arrowSlots[0].ArrowInSlot());
        yield return new WaitForSeconds(timeBetweenRewards);
        ClearRewardHighlight();

        ChangeState(WheelState.Idle);
    }

    void CheckRewardSegment(float evalAngle, Arrow triggeringArrow)
    {
        WheelSegment rewardSegment = RewardSegmentAtPoint(evalAngle);

        if (rewardSegment != null)
        {
            triggeringArrow.SegmentLandedUnderArrow(rewardSegment);
            if (triggeringArrow.InteractsWithSegmentUnderArrow())
            {
                HighlightRewardSegment(rewardSegment);
            }
        }
        else
        {
            Debug.LogWarning("No reward segment found!!!");
        }
    }

    void HighlightRewardSegment(WheelSegment seg)
    {
        highlightSegmentImage.transform.localEulerAngles = seg.transform.localEulerAngles;
        highlightSegmentImage.transform.localEulerAngles += Vector3.back * 5f;
        highlightSegmentImage.fillAmount = seg.SegmentActualSize() + highlightFillExcess;

        overlayImage.fillAmount = 1f - highlightSegmentImage.fillAmount;
        overlayImage.transform.localEulerAngles = highlightSegmentImage.transform.localEulerAngles;

        int sibs = segmentHolder.childCount;
        highlightSegmentImage.transform.SetSiblingIndex(sibs-1);
        seg.transform.SetSiblingIndex(sibs-1);
    }

    void ClearRewardHighlight()
    {
        highlightSegmentImage.fillAmount = 0f;
        overlayImage.fillAmount = 1f;
    }

    public WheelSegment RewardSegmentAtPoint(float evaluationAngle)
    {
        float evaluationPosition = TopOfWheelAngle();
        float checkedAngles = 0f;


        for (int i = 0; i < wheelSegments.Count; i++)
        {
            checkedAngles += wheelSegments[i].AngleOnWheel();
            Debug.Log("Checked Angles " + checkedAngles + " / EvaluationPos " + evaluationPosition);
            if (evaluationPosition <= checkedAngles)
            {
                Debug.Log("FOUND " + wheelSegments[i].SegColour().ToString() + " SEGMENT");
                return wheelSegments[i];
            }
        }

        return null;
    }

    public float WheelArrowAngle(WheelArrowClockPositions clockPos)
    {
        switch (clockPos)
        {
            case WheelArrowClockPositions.Twelve:
                return TopOfWheelAngle();
            case WheelArrowClockPositions.One:
                return AngelFromTopOfTheWheel(30f);
            case WheelArrowClockPositions.Two:
                return AngelFromTopOfTheWheel(60f);
            case WheelArrowClockPositions.Three:
                return AngelFromTopOfTheWheel(90f);
            case WheelArrowClockPositions.Four:
                return AngelFromTopOfTheWheel(120f);
            case WheelArrowClockPositions.Five:
                return AngelFromTopOfTheWheel(150f);
            case WheelArrowClockPositions.Six:
                return AngelFromTopOfTheWheel(180f);
            case WheelArrowClockPositions.Seven:
                return AngelFromTopOfTheWheel(210f);
            case WheelArrowClockPositions.Eight:
                return AngelFromTopOfTheWheel(240f);
            case WheelArrowClockPositions.Nine:
                return AngelFromTopOfTheWheel(270f);
            case WheelArrowClockPositions.Ten:
                return AngelFromTopOfTheWheel(300f);
            case WheelArrowClockPositions.Eleven:
                return AngelFromTopOfTheWheel(330f);
            default:
                return TopOfWheelAngle();
        }
    }

    float TopOfWheelAngle() { return wheel.localEulerAngles.z; }
    float AngelFromTopOfTheWheel(float angle)
    {
        float result = wheel.localEulerAngles.z + angle;
        if(result > 360f)
        {
            result -= 360f;
        }
        else if(result < 0)
        {
            result += 360f;
        }

        return result;
    }

    public float WheelRotationAngle() { return wheel.localEulerAngles.z; }

    void ChangeState(WheelState state) 
    { 
        if(currentState == state) return;

        //Leaving states events.
        switch (currentState)
        {
            case WheelState.Reward:
                RunLogger.main.CheckRewardTrends();
                break;
        }

        currentState = state;
    }

    public int WheelSize()
    {
        int size = 0;
        foreach (WheelSegment segment in wheelSegments) { size += segment.SegmentSize(); }
        return size;
    }

    IEnumerator AnimateWheelSpin(float duration)
    {
        float timeElapsed = 0f;
        float rotationSpeed = 0f;

        while (timeElapsed < duration)
        {
            if(timeElapsed < windUpDuration)
            {
                rotationSpeed = Mathf.Lerp(0, maxSpinSpeed, timeElapsed / windUpDuration);
            }
            else
            {
                rotationSpeed = maxSpinSpeed;
            }

            wheel.Rotate(Vector3.forward *  rotationSpeed * Time.deltaTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        while(timeElapsed < windUpDuration)
        {
            rotationSpeed = Mathf.Lerp(maxSpinSpeed, 0, timeElapsed / windUpDuration);

            wheel.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(ProcessRewards());
    }

    #region Tooltip
    [Header("ArrowTooltip")]
    [SerializeField] Transform tooltipBox;
    [SerializeField] Transform tooltipCanvas;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDesc, tooltipType, remainingUsesText;
    [SerializeField] Vector3[] toolTipSlotPositions;
    [SerializeField] Vector3 wheelViewPos, wheelViewRot;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] WheelCollider wheelCollider;
    bool focused;
    bool arrowPlacementMode;

    public void ToggleTooltip(bool toggle) { ToggleTooltip(toggle, arrowSlots[0]); }
    public void ToggleTooltip(bool toggle, ArrowSlot slot)
    {
        tooltipBox.gameObject.SetActive(toggle);
        if (toggle)
        {
            tooltipBox.position = slot.transform.position;

            Arrow arrow = slot.ArrowInSlot();

            tooltipTitle.text = arrow.ArrowName();
            tooltipDesc.text = arrow.ArrowDescriptions();
            tooltipType.text = arrow.ArrowTypes();

            int index = 0;
            for (int i = 1; i < arrowSlots.Length; i++) { if (arrowSlots[i] == slot) { index = i; break; } }
            tooltipCanvas.localPosition = toolTipSlotPositions[index];
        }
    }

    public void FocusIntoWheel()
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.Wheel);
        Player.local.MovePlayerToPos(wheelViewPos, wheelViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        wheelCollider.ToggleColliders(false);
        Cursor.lockState = CursorLockMode.Confined;

        foreach(ArrowSlot s in arrowSlots)
        {
            s.ToggleCollider(true);
        }
    }

    public void ArrowPlacementView()
    {
        arrowPlacementMode = true;
        Player.local.MovePlayerToPos(wheelViewPos, wheelViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        wheelCollider.ToggleColliders(false);
        Cursor.lockState = CursorLockMode.Confined;

        foreach (ArrowSlot s in arrowSlots)
        {
            s.ToggleCollider(true);
        }
    }

    public void LeaveArrowPlacementView()
    {
        arrowPlacementMode = false;
        wheelCollider.ToggleColliders(true);

        foreach (ArrowSlot s in arrowSlots)
        {
            s.ToggleCollider(false);
        }
    }

    void UnfocusWheel()
    {
        focused = false;
        Player.local.ReleaseControlOfCamera();
        wheelCollider.ToggleColliders(true);
        Cursor.lockState = CursorLockMode.Locked;

        foreach (ArrowSlot s in arrowSlots)
        {
            s.ToggleCollider(false);
        }
    }


    #endregion

    #region Reward
    [Header("RewardVisuals")]
    [SerializeField] GameObject fuelRewardPrefab;
    [SerializeField] Vector3[] fuelRewardPos;
    [SerializeField] GameObject coinRewardPrefab;
    [SerializeField] Vector3[] coinRewardPos;
    [SerializeField] float rewardDur;
    [SerializeField] float delayBetweenRewards;
    int coinsToSpawn = 0;
    int fuelToSpawn = 0;

    public void GainRewardResources(int coins, int fuel)
    {
        coinsToSpawn = coins;
        fuelToSpawn = fuel;

        StartCoroutine(AnimateSpawnRewardResources());

    }

    IEnumerator AnimateSpawnRewardResources()
    {
        int highestValue = Mathf.Max(coinsToSpawn, fuelToSpawn);

        while (highestValue > 0)
        {
            if(coinsToSpawn > 0) 
            {
                GameObject cObj = Instantiate(coinRewardPrefab);
                cObj.transform.SetParent(transform);
                StartCoroutine(MoveRewardObjectAlongPathThenDestroy(cObj.transform, coinRewardPos, rewardDur, RewardProfile.RewardType.Coins));
                coinsToSpawn--;
            }
            if (fuelToSpawn > 0)
            {
                GameObject fObj = Instantiate(fuelRewardPrefab);
                fObj.transform.SetParent(transform);
                StartCoroutine(MoveRewardObjectAlongPathThenDestroy(fObj.transform, fuelRewardPos, rewardDur, RewardProfile.RewardType.Fuel));
                fuelToSpawn--;
            }
            highestValue--;

            yield return new WaitForSeconds(delayBetweenRewards);
        }
    }

    IEnumerator MoveRewardObjectAlongPathThenDestroy(Transform obj, Vector3[] path, float dur, RewardProfile.RewardType rewardType)
    {
        float timeElapsed = 0f;

        while (timeElapsed < dur)
        {
            float t = timeElapsed / dur;
            int low = Mathf.FloorToInt((path.Length - 1) * t);
            t = ((path.Length - 1) * t) % 1;

            obj.localPosition = Vector3.Lerp(path[low], path[low + 1], t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        switch (rewardType)
        {
            case RewardProfile.RewardType.Fuel:
                RewardShoot.main.SpawnFuelReward(1);
                break;
            case RewardProfile.RewardType.Coins:
                CoinSpawner.main.SpawnCoinsAfterDelay(1);
                break;
        }

        Destroy(obj.gameObject);
    }

    #endregion
}
