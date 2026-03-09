using UnityEngine;

public class WheelCollider : Interactable
{
    public override void Interact()
    {
        Wheel.main.SpinTheWheel();
    }
}
