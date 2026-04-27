using System;
using UnityEngine;
using UnityEngine.UI;

public class ControllerButton : MonoBehaviour
{
    [SerializeField] GameObject activeButtonHighlight;
    [SerializeField] ParticleSystem activeButtonPS;
    [SerializeField] ControllerButton upButton, rightButton, downButton, leftButton;
    [Header("ButtonType")]
    [SerializeField] Button baseButton;
    [SerializeField] ButtonObject threeDButton;
    [SerializeField] Slider slider;
    [SerializeField] GameObject sliderHighlight;
    [SerializeField] Toggle toggle;
    bool isActiveFocus;
    bool sliderFocus;

    public virtual void GainFocus()
    {
        isActiveFocus = true;
        if (activeButtonHighlight != null) activeButtonHighlight.SetActive(true);
        if (activeButtonPS != null) activeButtonPS.Play();

        InputManager.main.ControllerEventActivate += PressButton;
        InputManager.main.ControllerEventDown += MoveFocusDown;
        InputManager.main.ControllerEventLeft += MoveFocusLeft;
        InputManager.main.ControllerEventRight += MoveFocusRight;
        InputManager.main.ControllerEventUp += MoveFocusUp;

        InputManager.InputDeviceChanged += OnInputDeviceChanged;
    }

    public virtual void LoseFocus()
    {
        if (sliderFocus) { UnfocusSlider(); }

        isActiveFocus = false;
        if (activeButtonHighlight != null) activeButtonHighlight.SetActive(false);
        if(activeButtonPS != null) activeButtonPS.Stop();

        InputManager.main.ControllerEventActivate -= PressButton;
        InputManager.main.ControllerEventDown -= MoveFocusDown;
        InputManager.main.ControllerEventLeft -= MoveFocusLeft;
        InputManager.main.ControllerEventRight -= MoveFocusRight;
        InputManager.main.ControllerEventUp -= MoveFocusUp;

        InputManager.InputDeviceChanged -= OnInputDeviceChanged;
    }

    void MoveFocusDown(object sender, EventArgs args)
    {
        if (downButton != null)
        {
            downButton.GainFocus();
            LoseFocus();
        }
    }

    void MoveFocusRight(object sender, EventArgs args)
    {
        if (rightButton != null)
        {
            rightButton.GainFocus();
            LoseFocus();
        }
    }

    void MoveFocusUp(object sender, EventArgs args)
    {
        if (upButton != null)
        {
            upButton.GainFocus();
            LoseFocus();
        }
    }

    void MoveFocusLeft(object sender, EventArgs args)
    {
        if (leftButton != null)
        {
            leftButton.GainFocus();
            LoseFocus();
        }
    }

    void PressButton(object sender, EventArgs args)
    {
        if(slider != null) 
        {
            if (sliderFocus) { UnfocusSlider(); }
            else { FocusOnSlider(); } 
            return; 
        }

        baseButton?.onClick.Invoke();
        threeDButton?.Activate();
        if(toggle != null) { toggle.isOn = !toggle.isOn; }
    }

    void FocusOnSlider()
    {
        sliderFocus = true;
        sliderHighlight.gameObject.SetActive(true);
        activeButtonHighlight.gameObject.SetActive(false);

        InputManager.main.ControllerEventLeft += MoveSliderDown;
        InputManager.main.ControllerEventRight += MoveSliderUp;

        InputManager.main.ControllerEventDeactivate += UnfocusSlider;
    }

    void MoveSliderDown(object sender, EventArgs args)
    {
        slider.value -= Time.deltaTime;
    }

    void MoveSliderUp(object sender, EventArgs args)
    {
        slider.value += Time.deltaTime;
    }

    void UnfocusSlider(object sender = null, EventArgs args = null)
    {
        sliderFocus = false;
        sliderHighlight.gameObject.SetActive(false); 
        activeButtonHighlight.gameObject.SetActive(true);

        InputManager.main.ControllerEventLeft -= MoveSliderDown;
        InputManager.main.ControllerEventRight -= MoveSliderUp;

        InputManager.main.ControllerEventDeactivate -= UnfocusSlider;
    }

    void OnInputDeviceChanged(InputManager.InputDevice newDevice)
    {
        if(newDevice != InputManager.InputDevice.Controller)
        {
            LoseFocus();
        }
    }

    private void OnDisable()
    {
        LoseFocus();
    }
}
