using Cinemachine;
using StarterAssets;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player local;

    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] CinemachineVirtualCamera cameraCinemachine;
    [SerializeField] InteractManager interactManager;
    Vector3 resetPos, resetRot;

    private void Awake()
    {
        local = this;
    }

    private void Start()
    {
        resetPos = GetPosition();
        resetRot = GetRotation();
    }

    public void ResetToStartPositions()
    {
        MovePlayerToPos(resetPos, resetRot);
    }

    public void TakeControlOfCamera(FirstPersonController.Controller controller)
    {
        if(controller != FirstPersonController.Controller.Player) { interactManager.ClearFocus(); }
        firstPersonController.SetController(controller);
    }

    public void ForceLookAt(Transform t)
    {
        cameraCinemachine.m_LookAt = t;
    }

    public void ReleaseLookAt()
    {
        cameraCinemachine.m_LookAt = null;
    }

    public void ReleaseControlOfCamera()
    {
        firstPersonController.SetController(FirstPersonController.Controller.Player);
        ReleaseLookAt();
    }

    public void MovePlayerToPos(Vector3 pos, Vector3 eulerRot)
    {
        firstPersonController.ManuallyMoveToPos(pos);
        firstPersonController.ManuallyMoveToRot(eulerRot);
    }
    public Vector3 GetPosition() { return firstPersonController.GetPosition(); }
    public Vector3 GetRotation() { return firstPersonController.GetRotation(); }
}
