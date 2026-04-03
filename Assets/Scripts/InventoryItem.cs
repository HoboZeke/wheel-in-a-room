using UnityEngine;

public class InventoryItem : Interactable
{
    public enum InvItem { Cog, Key }
    [SerializeField] InvItem item;

    public override void Interact()
    {
        PickupItem();
    }

    void PickupItem()
    {
        switch (item)
        {
            case InvItem.Cog:
                PlayerInventory.main.AddCogToInv();
                break;
            case InvItem.Key:
                PlayerInventory.main.AddKeyToInv();
                break;
        }

        Destroy(gameObject);
    }
}
