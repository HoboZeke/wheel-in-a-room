using System;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager main;

    public enum InputDevice { Keyboard, Controller, Touch }
    InputDevice activeDevice;
    public InputDevice ActiveDevice { get { return activeDevice; } }

    public static Action<InputDevice> InputDeviceChanged;

    public event EventHandler ControllerEventUp;
    public event EventHandler ControllerEventDown;
    public event EventHandler ControllerEventLeft;
    public event EventHandler ControllerEventRight;
    public event EventHandler ControllerEventActivate;

    public event EventHandler ControllerEventDeactivate;

    [SerializeField] bool busy;

    private void Awake()
    {
        main = this;
    }

    public bool BlockInteraction()
    {
        return busy || GameManager.main.IsPaused();
    }

    public void SetBusy(bool b) {  busy = b; }

    private void Update()
    {
        if(activeDevice != CheckInputDevice())
        {
            activeDevice = CheckInputDevice();
            if(activeDevice == InputDevice.Controller) { UIController.main.SwitchToControllerInput(); }
            InputDeviceChanged?.Invoke(activeDevice);
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !Menu.main.InMenu())
        {
            if (GameManager.main.IsPaused())
            {
                GameManager.main.UnpauseGame();
            }
            else
            {
                GameManager.main.PauseGame();
            }
        }

        if (activeDevice == InputDevice.Controller)
        {
            if (Input.GetButtonDown("Left")) { ControllerEventLeft?.Invoke(this, EventArgs.Empty); }
            else if (Input.GetButtonDown("Right")) { ControllerEventRight?.Invoke(this, EventArgs.Empty); }
            else if (Input.GetButtonDown("Up")) { ControllerEventUp?.Invoke(this, EventArgs.Empty); }
            else if (Input.GetButtonDown("Down")) { ControllerEventDown?.Invoke(this, EventArgs.Empty); }
            else if (Input.GetButtonDown("Activate")) { ControllerEventActivate?.Invoke(this, EventArgs.Empty); }
            else if (Input.GetButtonDown("Deactivate")) { ControllerEventDeactivate?.Invoke(this, EventArgs.Empty); }
        }
    }

    InputDevice CheckInputDevice()
    {
        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) { return InputDevice.Touch; }

        if (Input.GetJoystickNames().Length == 0) { return InputDevice.Keyboard; }

        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton1)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton2)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton3)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton4)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton5)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton6)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton7)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton8)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton9)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton10)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton11)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton12)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton13)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton14)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton15)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton16)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton17)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton18)) return InputDevice.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton19)) return InputDevice.Controller;
            else return InputDevice.Keyboard;
        }

        if (Input.anyKey)
        {
            // Unity will only recognize Input.anyKey for Keyboard.
            if (Input.GetAxisRaw("Horizontal") != 0) return InputDevice.Keyboard;
            if (Input.GetAxisRaw("Vertical") != 0) return InputDevice.Keyboard;
        }


        if (Input.GetAxisRaw("Horizontal") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("Vertical") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("Horizontal2") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("Vertical2") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("HorizontalD") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("VerticalD") != 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("LeftTrigger") < 0) return InputDevice.Controller;
        if (Input.GetAxisRaw("RightTrigger") < 0) return InputDevice.Controller;

        return activeDevice;
    }
}
