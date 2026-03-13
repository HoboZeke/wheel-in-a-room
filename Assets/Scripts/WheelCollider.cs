using UnityEngine;

public class WheelCollider : Interactable
{
    [SerializeField] Lever wheelLever;

    public override void Interact()
    {
        wheelLever.TriggerLever();
    }
}
