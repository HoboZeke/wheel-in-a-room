using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField] float interactionDistance;
    [SerializeField] Interactable activeTarget;

    private void Update()
    {
        if (InputManager.main.BlockInteraction()) { return; }
                    
        CastInteractionRay();

        if (Input.GetMouseButtonDown(0) && activeTarget != null)
        {
            activeTarget.Interact();
        }
    }

    void CastInteractionRay()
    {
        string debugstring = "None";
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, interactionDistance);

        if (hit.collider != null)
        {
            if (hit.collider.CompareTag("Interactable") && hit.collider.gameObject != activeTarget.gameObject)
            {
                activeTarget.OnLoseFocus();
                activeTarget = hit.collider.GetComponent<Interactable>();
                debugstring = hit.collider.gameObject.name;
                activeTarget.OnGainFocus();
            }
            else
            {
                if(activeTarget != null) activeTarget.OnLoseFocus();
                activeTarget = null;
            }
        }
        else
        {
            if (activeTarget != null) activeTarget.OnLoseFocus();

            activeTarget = null;
        }

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.cyan, 0.1f);
        //DebugUI.main.DebugLabel(debugstring);
    }

    public void ClearFocus()
    {
        if(activeTarget != null) { activeTarget.OnLoseFocus(); }
    }
}
