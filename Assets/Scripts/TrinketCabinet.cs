using TMPro;
using UnityEngine;

public class TrinketCabinet : Interactable
{
    public static TrinketCabinet main;

    [SerializeField] GameObject[] hooks;
    [SerializeField] int trinketLimit;
    [SerializeField] Vector3 trinketPlacementOffset;
    [SerializeField] GameObject[] trinketsInSlot;
    [SerializeField] Vector3 cabinetViewPos, cabinetViewRot;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] BoxCollider boxCollider;

    [Header("Tooltip")]
    [SerializeField] Transform tooltipBox;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDesc, tooltipType;
    bool focused = false;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    { 
        trinketsInSlot = new GameObject[hooks.Length];
        UpdateUnlockedSlots();
        HideTooltip();
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

        foreach(GameObject t in trinketsInSlot)
        {
            if(t != null)
            {
                t.GetComponent<TrinketObject>().ToggleCollider(true);
            }
        }
    }

    void UnfocusCabinet()
    {
        focused = false;
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;

        foreach (GameObject t in trinketsInSlot)
        {
            if (t != null)
            {
                t.GetComponent<TrinketObject>().ToggleCollider(false);
            }
        }
    }

    public void HideTooltip()
    {
        tooltipBox.gameObject.SetActive(false);
    }

    public void SetupTooltip(TrinketObject tObj)
    {
        tooltipBox.transform.position = tObj.transform.position;

        tooltipTitle.text = tObj.Trinket().TrinketName;
        tooltipDesc.text = tObj.Trinket().TrinketDescription;
        tooltipType.text = "";

        tooltipBox.gameObject.SetActive(true);

    }
}
