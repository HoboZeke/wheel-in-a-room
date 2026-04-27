using UnityEngine;

public class WheelCollider : Interactable
{
    [SerializeField] Wheel wheel;
    [SerializeField] BoxCollider[] colliders;

    public override void Interact()
    {
        wheel.GainFocus();
    }

    public void ToggleColliders(bool toggle)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = toggle;
        }
    }
}
