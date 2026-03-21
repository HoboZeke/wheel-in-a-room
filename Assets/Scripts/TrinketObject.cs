using UnityEngine;

public class TrinketObject : MonoBehaviour
{
    [SerializeField] MeshFilter trinketItemMeshFilter;
    [SerializeField] MeshRenderer trinketItemMeshRenderer;
    [SerializeField] Trinket connectedTrinket;

    public void SetupTrinket(Trinket t)
    {
        connectedTrinket = t;
        trinketItemMeshFilter.mesh = t.TrinketMesh;
        trinketItemMeshRenderer.material = t.TrinketMaterial;
    }
}
