using UnityEngine;

public class ArrowManager : MonoBehaviour
{
    public static ArrowManager main;

    [SerializeField] Shop shop;
    [SerializeField] GameObject arrowPrefab;
    [SerializeField] ArrowSlot[] slots;
    [SerializeField] Arrow[] arrowsInPositions;

    Arrow ghostArrow;

    private void Awake()
    {
        main = this;
    }

    public void CreateArrowToPlace(ArrowProfile profile)
    {
        ArrowSlot slot = FirstEmptySlot();
        if(slot == null) { Debug.LogError("Trying to place an arrow when there is no space!"); return; }

        GameObject arrowObj = Instantiate(arrowPrefab);
        Arrow a = arrowObj.GetComponent<Arrow>();

        ghostArrow = a;
        a.LoadProfile(profile);

        slot.PlaceArrowInSlot(a);
    }

    public void TryToPlaceGhost(ArrowSlot slot)
    {
        if (slot.IsEmpty() && ghostArrow != null)
        {
            ArrowSlot oldSlot = SlotContainingArrow(ghostArrow);
            if (oldSlot != null) { oldSlot.EmptySlot(); }

            slot.PlaceArrowInSlot(ghostArrow);
        }
    }

    public void SelectArrowSlot(ArrowSlot slot)
    {
        if(slot == SlotContainingArrow(ghostArrow))
        {
            ghostArrow = null;
            shop.RevertAltFocus();
        }
    }

    public void RemoveArrow(Arrow arrow)
    {
        for (int i = 0; i < arrowsInPositions.Length; i++)
        {
            if(arrowsInPositions[i] == arrow)
            {
                arrowsInPositions[i] = null;
                slots[i].EmptySlot();
                return;
            }
        }
    }

    ArrowSlot FirstEmptySlot()
    {
        for(int i = 0;i < slots.Length; i++)
        {
            if (slots[i].IsEmpty()) { return slots[i]; }
        }

        return null;
    }

    ArrowSlot SlotContainingArrow(Arrow arrow)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].ArrowInSlot() == arrow) { return slots[i]; }
        }
        return null;
    }

}
