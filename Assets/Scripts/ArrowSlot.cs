using System;
using UnityEngine;

public class ArrowSlot : MonoBehaviour
{
    [SerializeField] Arrow arrowInSlot;
    [SerializeField] BoxCollider slotCollider;
    [SerializeField] Wheel.WheelArrowClockPositions positionOnWheel;

    private void OnMouseDown()
    {
        ArrowManager.main.SelectArrowSlot(this);
    }

    private void OnMouseEnter()
    {
        if (arrowInSlot != null)
        {
            Wheel.main.ToggleTooltip(true, this);
        }
        else
        {
            ArrowManager.main.TryToPlaceGhost(this);
        }
    }

    private void OnMouseExit()
    {
        Wheel.main.ToggleTooltip(false, this);
    }

    public void ToggleCollider(bool toggle)
    {
        slotCollider.enabled = toggle;
    }

    public void PlaceArrowInSlot(Arrow arrow)
    {
        arrowInSlot = arrow;
        arrow.transform.SetParent(transform);
        arrow.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        arrow.transform.localScale = Vector3.one;
        arrow.SetPositionOnWheel(positionOnWheel);
    }

    public Arrow ArrowInSlot() { return arrowInSlot; }
    public bool HasArrow() { return arrowInSlot != null; }

    public void EmptySlot() { arrowInSlot = null; }
    public bool IsEmpty() {  return arrowInSlot == null; }

    public Wheel.WheelArrowClockPositions ClockPosition() { return positionOnWheel; }
}
