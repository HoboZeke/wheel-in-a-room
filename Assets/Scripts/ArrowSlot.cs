using UnityEngine;

public class ArrowSlot : MonoBehaviour
{
    [SerializeField] Arrow arrowInSlot;
    [SerializeField] BoxCollider slotCollider;

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
    }

    public Arrow ArrowInSlot() { return arrowInSlot; }

    public void EmptySlot() { arrowInSlot = null; }
    public bool IsEmpty() {  return arrowInSlot == null; }
}
