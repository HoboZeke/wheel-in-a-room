using TMPro;
using UnityEngine;

public class Shop : Interactable
{
    [SerializeField] ShopSlot[] shopSlots;
    ShopSlot activeSlot;
    [SerializeField] Vector3 shopViewPos, shopViewRot;
    [SerializeField] BoxCollider boxCollider;
    [Header("Tooltip")]
    [SerializeField] Transform tooltipBox;
    [SerializeField] TextMeshPro tooltipTitle, tooltipDesc;
    bool focused;

    public override void Interact()
    {
        if (!focused)
        {
            FocusIntoShop();
        }
        else
        {
            UnfocusShop();
        }
    }

    void FocusIntoShop() 
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.Shop);
        Player.local.MovePlayerToPos(shopViewPos, shopViewRot);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void UnfocusShop()
    {
        focused = false;
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && focused)
        {
            UnfocusShop();
        }
    }

    public void SelectActiveShopSlot(ShopSlot slot)
    {
        if(slot == null)
        {
            activeSlot = null;
            tooltipBox.gameObject.SetActive(false);
        }
        else if(slot != activeSlot)
        {
            activeSlot = slot;
            tooltipBox.gameObject.SetActive(true);
            ShopItem item = slot.ItemInSlot();
            tooltipTitle.text = item.ItemName;
            tooltipDesc.text = item.ItemDescription;
        }
    }
}
