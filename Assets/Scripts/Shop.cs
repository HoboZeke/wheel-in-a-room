using UnityEngine;

public class Shop : Interactable
{
    [SerializeField] ShopSlot[] shopSlots;
    [SerializeField] Vector3 shopViewPos, shopViewRot;
    [SerializeField] BoxCollider boxCollider;
    bool focused;

    public override void Interact()
    {
        if (!focused)
        {
            focused = true;
            Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.Shop);
            Player.local.MovePlayerToPos(shopViewPos, shopViewRot);
            boxCollider.enabled = false;
        }
        else
        {
            Player.local.ReleaseControlOfCamera();
            boxCollider.enabled = true;
            focused = false;
        }
    }
}
