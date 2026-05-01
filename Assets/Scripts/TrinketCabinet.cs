using TMPro;
using UnityEngine;

public class TrinketCabinet : Focussable
{
    public static TrinketCabinet main;

    [Header("Trinket Cabinet")]
    [SerializeField] GameObject[] hooks;
    [SerializeField] int trinketLimit;
    [SerializeField] Vector3 trinketPlacementOffset;
    [SerializeField] GameObject[] trinketsInSlot;

    [Header("Tooltip")]
    [SerializeField] Transform tooltipBox;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDesc, tooltipType;

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
        GainFocus();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && focused)
        {
            LoseFocus();
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

    public override void GainFocus()
    {
        base.GainFocus();
        //foreach (GameObject t in trinketsInSlot)
        //{
        //    if (t != null)
        //    {
        //        t.GetComponent<TrinketObject>().ToggleCollider(true);
        //    }
        //}
    }

    public override void LoseFocus()
    {
        base.LoseFocus();
        //foreach (GameObject t in trinketsInSlot)
        //{
        //    if (t != null)
        //    {
        //        t.GetComponent<TrinketObject>().ToggleCollider(false);
        //    }
        //}
    }

    public bool IsFocused()
    {
        return focused;
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
