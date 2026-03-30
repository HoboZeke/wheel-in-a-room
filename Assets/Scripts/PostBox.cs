using UnityEngine;

public class PostBox : Interactable
{
    [SerializeField] PostOption[] boxOptions;
    [SerializeField] string[] postMessages;

    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] Vector3 boxViewPos, boxViewRot;
    bool focused = false;


    public override void Interact()
    {
        if (HasPost())
        {
            FocusOnPost();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && focused)
        {
            ExitPost();
        } 
    }

    public bool HasPost()
    {
        return boxOptions[0] != null;
    }

    public void FocusOnPost()
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.PostBox);
        Player.local.MovePlayerToPos(boxViewPos, boxViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void ExitPost()
    {
        focused = false;
        EmptyPost();
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PopulatePost()
    {
        for (int i = 0; i < boxOptions.Length; i++)
        {
            boxOptions[i] = Archive.main.PullPostFromPool();
        }
    }

    public void EmptyPost()
    {
        for(int i = 0;i < boxOptions.Length; i++)
        {
            if(boxOptions[i] != null)
            {
                Archive.main.AddToPostPool(boxOptions[i]);
                boxOptions[i] = null;
            }
        }
    }
}
