using UnityEngine;

public class TrinketCabinet : Interactable
{
    [SerializeField] GameObject[] hooks;
    [SerializeField] int trinketLimit;
    [SerializeField] Vector3 trinketPlacementOffset;
    [SerializeField] GameObject[] trinketsInSlot;
    [SerializeField] Vector3 cabinetViewPos, cabinetViewRot;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] BoxCollider boxCollider;
    bool focused = false;

    private void Start()
    { 
        trinketsInSlot = new GameObject[hooks.Length];
        UpdateUnlockedSlots();
    }

    public override void Interact()
    {
        FocusIntoCabinet();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && focused)
        {
            UnfocusCabinet();
        }
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

    void FocusIntoCabinet()
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.TrinketCabinet);
        Player.local.MovePlayerToPos(cabinetViewPos, cabinetViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void UnfocusCabinet()
    {
        focused = false;
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }
}
