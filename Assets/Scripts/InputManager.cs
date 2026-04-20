using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager main;

    [SerializeField] bool busy;

    private void Awake()
    {
        main = this;
    }

    public bool BlockInteraction()
    {
        return busy;
    }

    public void SetBusy(bool b) {  busy = b; }
}
