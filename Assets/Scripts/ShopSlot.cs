using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopSlot : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Shop shop;
    [SerializeField] GameObject shopItemVisual;
    [SerializeField] ShopItem itemInSlot;
    [SerializeField] int shopItemCost;
    [SerializeField] TextMeshProUGUI shopCostLabel;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        shop.SelectActiveShopSlot(this);
    }

    public ShopItem ItemInSlot() { return itemInSlot; }
}
