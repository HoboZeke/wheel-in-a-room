using UnityEngine;

public class HammerMount : Interactable
{
    [SerializeField] GameObject hammer;
    [SerializeField] BoxCollider boxCollider;

    public override void Interact()
    {
        PlayerInventory.main.AddHammerToInv();
        hammer.gameObject.SetActive(false);
        boxCollider.enabled = false;
    }
}
