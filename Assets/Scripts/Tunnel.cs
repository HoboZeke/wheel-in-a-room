using UnityEngine;

public class Tunnel : MonoBehaviour
{
    [SerializeField] Transform[] tunnelPieces;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxZ, minZ, lengthOfTunnel;

    private void Update()
    {
        float move = moveSpeed * Time.deltaTime;

        foreach(Transform t in tunnelPieces)
        {
            t.localPosition += Vector3.forward * move;

            if (moveSpeed > 0)
            {
                if (t.localPosition.z >= maxZ)
                {
                    t.localPosition -= Vector3.forward * lengthOfTunnel;
                }
            }
            else
            {
                if (t.localPosition.z <= minZ)
                {
                    t.localPosition += Vector3.forward * lengthOfTunnel;
                }
            }
        }
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public float Speed() { return moveSpeed; }
}
