using UnityEngine;

public class BreakglassCabinet : Interactable
{
    [SerializeField] GameObject glass;
    [SerializeField] GameObject wheel;
    [SerializeField] ParticleSystem glassFragementsPS;
    [SerializeField] BoxCollider boxCollider;

    public void ResetToStart()
    {
        glass.SetActive(true);
        wheel.SetActive(true);
    }

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

        UIController.main.PickupItem("miniwheel");

        Debug.Log("Picked up wheel");
    }
}
