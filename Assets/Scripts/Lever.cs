using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Lever : Interactable
{
    [SerializeField] Transform leverT;
    [SerializeField] UnityEvent leverEvent;
    [SerializeField] Vector3 baseEuler, pulledEuler;
    [SerializeField] float animDuration;
    [SerializeField] bool limitedBySpins;
    bool busy = false;

    private void OnMouseDown()
    {
        TriggerLever();
    }

    public override void Interact()
    {
        TriggerLever();
    }

    public void TriggerLever()
    {
        if(limitedBySpins && ProgressTracker.main.SpinsLeft() <= 0) { return; }
        if (!busy)
        {
            StartCoroutine(AnimateLeverPull(animDuration));
        }
    }

    IEnumerator AnimateLeverPull(float dur)
    {
        busy = true;
        float timeElapsed = 0f;
        bool triggered = false;

        while(timeElapsed < dur)
        {
            if(timeElapsed < dur / 2)
            {
                leverT.localRotation = Quaternion.Lerp(Quaternion.Euler(baseEuler), Quaternion.Euler(pulledEuler), timeElapsed/(dur/2));
            }
            else
            {
                if (!triggered)
                {
                    leverEvent?.Invoke();
                }
                leverT.localRotation = Quaternion.Lerp(Quaternion.Euler(pulledEuler), Quaternion.Euler(baseEuler), (timeElapsed - (dur/2)) / (dur / 2));
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        leverT.localEulerAngles = baseEuler;
        busy = false;
    }
}
