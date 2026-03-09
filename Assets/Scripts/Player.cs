using StarterAssets;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player local;

    [SerializeField] FirstPersonController firstPersonController;

    private void Awake()
    {
        local = this;
    }

    public void TakeControlOfCamera(FirstPersonController.Controller controller)
    {
        firstPersonController.SetController(controller);
    }

    public void ReleaseControlOfCamera()
    {
        firstPersonController.SetController(FirstPersonController.Controller.Player);
    }

    public void MovePlayerToPos(Vector3 pos, Vector3 eulerRot)
    {
        firstPersonController.ManuallyMoveToPos(pos);
        firstPersonController.ManuallyMoveToRot(eulerRot);
    }
}
