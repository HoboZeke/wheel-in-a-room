using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] Shop shop;
    [SerializeField] GameObject shopItemVisual;
    [SerializeField] MeshFilter shopItemMeshFilter;
    [SerializeField] MeshRenderer shopItemMeshRenderer;
    [SerializeField] ShopItem itemInSlot;
    [SerializeField] int shopItemCost;
    [SerializeField] TextMeshProUGUI shopCostLabel;

    void OnMouseDown()
    {
        Debug.Log("Pointer down on shop slot");
        shop.SelectActiveShopSlot(this);
    }

    public ShopItem ItemInSlot() { return itemInSlot; }

    public void PutItemInSlot(ShopItem shopItem)
    {
        itemInSlot = shopItem;
        shopItemCost = shopItem.ItemCost;
        shopCostLabel.text = shopItemCost + "g";

        shopItemVisual.SetActive(true);
        shopItemMeshFilter.mesh = shopItem.ItemMesh;
        shopItemMeshRenderer.material = shopItem.ItemMaterial;
        shopItemVisual.transform.localEulerAngles = shopItem.ItemEulerAngles;
    }

    public void EmptySlot()
    {
        itemInSlot = null;
        shopItemCost = 0;
        shopCostLabel.text = "Empty";

        shopItemVisual.SetActive(false);
    }
}
