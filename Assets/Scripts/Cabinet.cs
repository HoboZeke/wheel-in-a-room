using System.Collections;
using UnityEngine;

public class Cabinet : MonoBehaviour
{
    [SerializeField] Transform hinge;
    [SerializeField] Vector3 closeHingeRot, openHingeRot;
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] float animationDuration;
    bool opened;

    private void Start()
    {
        TrinketManager.main.OnWheelSpun += CheckConditionalOpen;
    }

    void CheckConditionalOpen(object sender, TrinketEventArgs args)
    {
        if (!opened)
        {
            if(Wheel.main.IsArrowPositionOccupied(Wheel.WheelArrowClockPositions.Twelve) && 
                Wheel.main.IsArrowPositionOccupied(Wheel.WheelArrowClockPositions.Two) &&
                Wheel.main.IsArrowPositionOccupied(Wheel.WheelArrowClockPositions.Seven))
            {
                OpenCabinet();
            }
        }
        else
        {
            TrinketManager.main.OnWheelSpun -= CheckConditionalOpen;
        }
    }

    void OpenCabinet()
    {
        TrinketManager.main.OnWheelSpun -= CheckConditionalOpen;

        StartCoroutine(HingeAnimation(closeHingeRot, openHingeRot, animationDuration));
        boxCollider.enabled = false;

    }

    IEnumerator HingeAnimation(Vector3 startRot, Vector3 endRot, float dur)
    {
        Quaternion start = Quaternion.Euler(startRot);
        Quaternion end = Quaternion.Euler(endRot);
        float timeElapsed = 0f;

        while(timeElapsed < dur)
        {
            hinge.localRotation = Quaternion.Lerp(start, end, timeElapsed/dur);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        hinge.localRotation = end;
    }
}
