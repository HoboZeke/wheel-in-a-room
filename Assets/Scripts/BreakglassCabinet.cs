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
        glassFragementsPS.Play();

        PlayerInventory.main.RemoveHammerFromInv();

        Debug.Log("BROKE GLASS!");
    }

    void PickupWheel()
    {
        wheel.SetActive(false);
        Wheel.main.AddMiniWheel();

        Debug.Log("Picked up wheel");
    }
}
