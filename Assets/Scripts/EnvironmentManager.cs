using UnityEngine;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager main;

    [SerializeField] Tunnel tunnel;

    private void Awake()
    {
        main = this;
    }

    public void SetTunnelSpeed(float speed)
    {
        tunnel.SetSpeed(speed);
    }

    public void MultiplyTunnelSpeed(float multi)
    {
        SetTunnelSpeed(tunnel.Speed() * multi);
    }


}
