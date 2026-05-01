using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrinketObject : Interactable
{
    [SerializeField] Transform trinketModelObject;
    [SerializeField] MeshFilter trinketItemMeshFilter;
    [SerializeField] MeshRenderer trinketItemMeshRenderer;
    [SerializeField] Trinket connectedTrinket;
    [SerializeField] BoxCollider boxCollider;

    [SerializeField] Transform tooltip;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipType, tooltipDescription;

    public void SetupTrinket(Trinket t)
    {
        connectedTrinket = t;
        trinketItemMeshFilter.mesh = t.TrinketMesh;
        trinketItemMeshRenderer.material = t.TrinketMaterial;
        if (t.TrinketMesh.subMeshCount > 1) 
        {
            List<Material> mats = new List<Material>();
            for (int i = 0; i < t.TrinketMesh.subMeshCount; i++) { mats.Add(t.TrinketMaterial); }
            trinketItemMeshRenderer.SetMaterials(mats);
        }

        trinketModelObject.localPosition = t.TrinketOffset();
        trinketModelObject.localScale = t.TrinketScale();
        trinketModelObject.localEulerAngles = t.TrinketEuler();

        tooltipTitle.text = t.TrinketName;
        tooltipType.text = t.TrinketTypeString;
        tooltipDescription.text = t.TrinketDescription;
    }

    public void ToggleCollider(bool b) { boxCollider.enabled = b; }

    public Trinket Trinket() { return connectedTrinket; }

    private void OnMouseExit()
    {
        if (!TrinketCabinet.main.IsFocused()) { return; }
        TrinketCabinet.main.HideTooltip();
    }

    private void OnMouseEnter()
    {
        if (!TrinketCabinet.main.IsFocused()) { return; }
        TrinketCabinet.main.SetupTooltip(this);
    }

    public override void OnGainFocus()
    {
        if (TrinketCabinet.main.IsFocused()) { return; }
        tooltip.LookAt(Camera.main.transform.position);
        tooltip.gameObject.SetActive(true);
    }

    public override void OnLoseFocus()
    {
        tooltip.gameObject.SetActive(false);
    }
}
