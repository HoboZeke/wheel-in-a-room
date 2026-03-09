using UnityEngine;

public class InteractManager : MonoBehaviour
{
    [SerializeField] float interactionDistance;
    [SerializeField] Interactable activeTarget;

    private void Update()
    {
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
            if (hit.collider.CompareTag("Interactable"))
            {
                activeTarget = hit.collider.GetComponent<Interactable>();
                debugstring = hit.collider.gameObject.name;
            }
            else
            {
                activeTarget = null;
            }
        }
        else
        {
            activeTarget = null;
        }

        Debug.DrawRay(transform.position, transform.forward * interactionDistance, Color.cyan, 0.1f);
        //DebugUI.main.DebugLabel(debugstring);
    }
}
