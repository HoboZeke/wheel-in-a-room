using UnityEngine;

public class TrinketCabinet : MonoBehaviour
{
    [SerializeField] GameObject[] hooks;
    [SerializeField] int trinketLimit;
    [SerializeField] Vector3 trinketPlacementOffset;
    [SerializeField] GameObject[] trinketsInSlot;

    private void Start()
    { 
        trinketsInSlot = new GameObject[hooks.Length];
        UpdateUnlockedSlots();
    }

    void UpdateUnlockedSlots()
    {
        for (int i = 0; i < hooks.Length; i++)
        {
            hooks[i].SetActive(i < trinketLimit);
        }
    }

    public bool HasSpace()
    {
        foreach (GameObject go in trinketsInSlot)
        {
            if(go == null) { return true; }
        }

        return false;
    }

    public void AddTrinketToCabinet(GameObject trinket)
    {
        for (int i = 0; i < trinketsInSlot.Length; i++)
        {
            if (trinketsInSlot[i] == null)
            {
                PlaceTrinketInSlot(i,trinket);
                return;
            }
        }
    }

    void PlaceTrinketInSlot(int slot, GameObject trinket)
    {
        if (trinketsInSlot[slot] != null) { return; }

        trinket.transform.SetParent(transform);

        trinketsInSlot[slot] = trinket;
        trinket.transform.localPosition = hooks[slot].transform.localPosition + trinketPlacementOffset;
    }
}
