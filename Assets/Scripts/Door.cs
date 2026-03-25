using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Door : Interactable
{
    [SerializeField] Vector3[] openPositions;
    [SerializeField] Vector3[] openRotations;
    [SerializeField] float[] openAnimDurations;

    public override void Interact()
    {
        OpenDoor();
    }

    [ContextMenu("LogPosAndRot")]
    void LogCurrentPositionAndRotation()
    {
        Vector3[] newPositions = new Vector3[openPositions.Length + 1];
        Vector3[] newRots = new Vector3[openRotations.Length + 1];

        for (int i = 0; i < openPositions.Length; i++)
        {
            newPositions[i] = openPositions[i];
            newRots[i] = openRotations[i];
        }

        newPositions[newPositions.Length - 1] = transform.localPosition;
        newRots[newRots.Length - 1] = transform.localEulerAngles;

        openPositions = newPositions;
        openRotations = newRots;
    }

    [ContextMenu("ResetToFirstPos")]
    void ResetToFirstPos()
    {
        transform.localPosition = openPositions[0];
        transform.localEulerAngles = openRotations[0];
    }

    void OpenDoor()
    {
        StartCoroutine(OpenDoorAnimations());
    }

    IEnumerator OpenDoorAnimations()
    {
        float timeElapsed = 0f;
        int step = 0;

        while(step < openPositions.Length - 1)
        {
            float t = timeElapsed / openAnimDurations[step];

            transform.localPosition = Vector3.Lerp(openPositions[step], openPositions[step+1], t);
            transform.localRotation = Quaternion.Lerp(Quaternion.Euler(openRotations[step]), Quaternion.Euler(openRotations[step + 1]), t);
            
            timeElapsed += Time.deltaTime;
            if (timeElapsed > openAnimDurations[step])
            {
                step++;
                timeElapsed = 0f;
            }
            yield return null;
        }

        transform.localPosition = openPositions[openPositions.Length - 1];
        transform.localEulerAngles = openRotations[openRotations.Length - 1];
    }
}
