using UnityEngine;

public class BreakglassCabinet : Interactable
{
    [SerializeField] GameObject glass;
    [SerializeField] GameObject wheel;
    [SerializeField] ParticleSystem glassFragementsPS;
    [SerializeField] BoxCollider boxCollider;

    public override void Interact()
    {
        if (PlayerInventory.main.HasHammer() && glass.activeInHierarchy)
        {
            BreakGlass();
        }
        else if (wheel.activeInHierarchy && !glass.activeInHierarchy)
        {
            PickupWheel();
        }
    }

    void BreakGlass()
    {
        glass.SetActive(false);
        boxCollider.enabled = false;
        glassFragementsPS.Play();

        PlayerInventory.main.RemoveHammerFromInv();
    }

    void PickupWheel()
    {
        wheel.SetActive(false);
        Wheel.main.AddMiniWheel();
    }
}
