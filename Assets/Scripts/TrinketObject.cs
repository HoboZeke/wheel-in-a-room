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
            for (int i = 1; i < t.TrinketMesh.subMeshCount; i++) { trinketItemMeshRenderer.materials[i] = t.TrinketMaterial; }
        }

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
        TrinketCabinet.main.HideTooltip();
    }

    private void OnMouseEnter()
    {
        TrinketCabinet.main.SetupTooltip(this);
    }

    public override void OnGainFocus()
    {
        tooltip.LookAt(Player.local.GetPosition());
        tooltip.gameObject.SetActive(true);
    }

    public override void OnLoseFocus()
    {
        tooltip.gameObject.SetActive(false);
    }
}
