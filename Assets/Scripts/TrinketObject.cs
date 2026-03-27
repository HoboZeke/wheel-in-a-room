using UnityEngine;

public class TrinketObject : MonoBehaviour
{
    [SerializeField] MeshFilter trinketItemMeshFilter;
    [SerializeField] MeshRenderer trinketItemMeshRenderer;
    [SerializeField] Trinket connectedTrinket;
    [SerializeField] BoxCollider boxCollider;

    public void SetupTrinket(Trinket t)
    {
        connectedTrinket = t;
        trinketItemMeshFilter.mesh = t.TrinketMesh;
        trinketItemMeshRenderer.material = t.TrinketMaterial;
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
}
