using UnityEngine;

public class WheelCollider : Interactable
{
    [SerializeField] Wheel wheel;
    [SerializeField] BoxCollider[] colliders;

    public override void Interact()
    {
        wheel.FocusIntoWheel();
    }

    public void ToggleColliders(bool toggle)
    {
        foreach (Collider collider in colliders)
        {
            collider.enabled = toggle;
        }
    }
}
