using StarterAssets;
using System;
using UnityEngine;

public class Focussable : Interactable
{
    [Header("Focussable")]
    public Transform cameraFocalPoint;
    [SerializeField] FirstPersonController.Controller cameraOwner;
    public Vector3 viewPos, viewRot;
    [SerializeField] BoxCollider boxCollider;
    public bool focused;

    [SerializeField] Focussable leftNeighbour, rightNeighbour;

    public override void Interact()
    {
        if (!focused)
        {
            GainFocus();
        }
        else
        {
            LoseFocus();
        }
    }

    public virtual void GainFocus()
    {       
        focused = true;
        Player.local.TakeControlOfCamera(cameraOwner);
        Player.local.MovePlayerToPos(viewPos, viewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        SetupListeners();
    }

    public virtual void LoseFocus()
    {
        focused = false;
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        ClearListeners();
    }

    void SetupListeners()
    {
        InputManager.main.ControllerEventRightTab += TryMoveRight;
        InputManager.main.ControllerEventLeftTab += TryMoveLeft;
    }

    void ClearListeners()
    {
        InputManager.main.ControllerEventRightTab -= TryMoveRight;
        InputManager.main.ControllerEventLeftTab -= TryMoveLeft;
    }

    private void OnDestroy()
    {
        ClearListeners();
    }

    void TryMoveRight(object sender, EventArgs eventArgs)
    {
        if(rightNeighbour != null) { MoveRight(); }
    }

    void TryMoveLeft(object sender, EventArgs eventArgs)
    {
        if (leftNeighbour != null) { MoveLeft(); }
    }

    public virtual void MoveLeft()
    {
        LoseFocus();
        leftNeighbour.GainFocus();
    }

    public virtual void MoveRight()
    {
        LoseFocus();
        rightNeighbour.GainFocus();
    }
}
