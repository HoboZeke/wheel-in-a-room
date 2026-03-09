using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour
{
    public static Wheel main;

    [SerializeField] Transform wheel;
    [SerializeField] Transform segmentCanvas;
    [SerializeField] GameObject wheelSegmentPrefab;
    public enum WheelState { Idle, Spinning, Reward }
    [SerializeField] WheelState currentState;
    List<WheelSegment> wheelSegments = new List<WheelSegment>();

    [Header("SpinSettings")]
    [SerializeField] float maxSpinSpeed;
    [SerializeField] float windUpDuration;
    [SerializeField] Vector2 spinDuration;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        AddSegment(3, null, Color.azure);
        AddSegment(2, null, Color.orangeRed);
        AddSegment(1, null, Color.green);
    }

    void AddSegment(int size, RewardProfile reward, Color color)
    {
        GameObject obj = Instantiate(wheelSegmentPrefab);
        obj.transform.SetParent(segmentCanvas);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;

        WheelSegment seg = obj.GetComponent<WheelSegment>();
        wheelSegments.Add(seg);
        seg.Setup(size, color, reward);
        AlignAllSegments();
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

        ChangeState(WheelState.Spinning);
        StartCoroutine(AnimateWheelSpin(Random.Range(spinDuration.x, spinDuration.y)));
    }

    void RewardWheelPosition()
    {
        ChangeState(WheelState.Reward);

        WheelSegment rewardSegment = RewardSegmentAtPoint(TopOfWheelAngle());

        if (rewardSegment != null)
        {
            rewardSegment.GainReward();
        }
        else
        {
            Debug.LogWarning("No reward segment found!!!");
        }
    }

    WheelSegment RewardSegmentAtPoint(float evaluationAngle)
    {
        float evaluationPosition = TopOfWheelAngle() * -1f;
        float checkedAngles = 0f;


        for (int i = 0; i < wheelSegments.Count; i++)
        {
            checkedAngles += wheelSegments[i].AngleOnWheel();
            if (checkedAngles <= evaluationPosition)
            {
                return wheelSegments[i];
            }
        }

        return null;
    }

    float TopOfWheelAngle() { return wheel.localEulerAngles.z; }

    public float WheelRotationAngle() { return wheel.localEulerAngles.z; }

    void ChangeState(WheelState state) 
    { 
        if(currentState == state) return; 
        
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

        RewardWheelPosition();
    }
}
