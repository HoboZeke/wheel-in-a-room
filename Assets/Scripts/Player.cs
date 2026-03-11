using Cinemachine;
using StarterAssets;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player local;

    [SerializeField] FirstPersonController firstPersonController;
    [SerializeField] CinemachineVirtualCamera cameraCinemachine;

    private void Awake()
    {
        local = this;
    }

    public void TakeControlOfCamera(FirstPersonController.Controller controller)
    {
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
}
