using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController main;

    #region Pickup
    [Header("Pick up Item Focus")]
    [SerializeField] GameObject pickupItemScreen;
    [SerializeField] UIFocusCamera uIFocusCamera;
    [SerializeField] TextMeshProUGUI itemTitleText, itemDescriptionText;
    bool waitingForInput;

    public void PickupItem(string itemName)
    {
        switch (itemName)
        {
            case "miniwheel":
                uIFocusCamera.ToggleMiniWheel(true);
                pickupItemScreen.SetActive(true);
                itemTitleText.text = "Mini Wheel";
                itemDescriptionText.text = "Adds a smaller wheel to the wheel which multiplies your winnings.";
                StartCoroutine(ClosePickupScreenAfterInput());
                break;
        }
    }

    IEnumerator ClosePickupScreenAfterInput()
    {
        waitingForInput = true;
        yield return new WaitForSeconds(0.1f);
        while (waitingForInput) { yield return null; }
        pickupItemScreen.SetActive(false);
        uIFocusCamera.TurnOffAll();
    }
    #endregion

    #region Post
    [Header("Post")]
    [SerializeField] PostBox postBox;
    [SerializeField] GameObject postScreen;
    [SerializeField] ControllerButton defaultPostControllerButton;
    [SerializeField] TextMeshProUGUI postTitleText, postFluffText, postMemoText;
    [SerializeField] UIPostOptionPanel[] postOptionsPanels;

    [ContextMenu("SetPanelIndices")]
    void SetPanelIndices() { for(int i = 0; i < postOptionsPanels.Length; i++) { postOptionsPanels[i].SetIndex(i); } }

    public void ShowPostScreen(string title, string fluff, PostOption[] options)
    {
        postTitleText.text = title;
        postFluffText.text = fluff;

        for(int i = 0; i < options.Length; i++)
        {
            postOptionsPanels[i].SetupPanel(options[i].PostName, options[i].PostDescription);
        }

        postScreen.SetActive(true);

        if(InputManager.main.ActiveDevice == InputManager.InputDevice.Controller) { defaultGameOverControllerButton.GainFocus(); }
    }

    public void HidePostScreen()
    {
        postScreen.SetActive(false);
    }

    public void SelectPostOption(int index)
    {
        postBox.ChoosePostReward(index);
        HidePostScreen();
    }
    #endregion

    #region Countdown
    [Header("Countdown")]
    [SerializeField] GameObject countDownScreen;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] Image countDownBackground;

    public void ToggleCountDownUI(bool toggle)
    {
        countDownScreen.SetActive(toggle);
        SetCountdownBackgroundAlpha(0f);
    }
    public void CountDownUI(string text)
    {
        countDownText.text = text;
    }

    public void SetCountdownBackgroundAlpha(float alpha)
    {
        countDownBackground.color = new Color(countDownBackground.color.r, countDownBackground.color.g, countDownBackground.color.b, alpha);
    }
    #endregion

    #region GameOver
    [Header("GameOver")]
    [SerializeField] GameObject gameOverScreen;
    [SerializeField] GameObject xMarker;
    [SerializeField] ControllerButton defaultGameOverControllerButton;
    [SerializeField] float gameOverScreenDuration;

    [SerializeField] Slider gameOverSlider;
    [SerializeField] Transform underLine, trainLineScreen;
    [SerializeField] RectTransform gameOverText;
    [SerializeField] float lineRevealTime;
    [SerializeField] Vector3 underlineFinishScale;
    [SerializeField] Vector3 gameOverTextStartPos, gameOverTextEndPos;


    void SetupGameOverValues()
    {
        underLine.localScale = Vector3.one;
        trainLineScreen.localScale = new Vector3(1f, 0f, 1f);
        gameOverSlider.value = 0f;
        gameOverText.anchoredPosition = gameOverTextStartPos;
        xMarker.gameObject.SetActive(false);
    }

    public void GameOverUI()
    {
        ToggleCountDownUI(false);
        gameOverScreen.SetActive(true);
        StartCoroutine(RevealTravelLine());
    }

    public void GoAgainButton()
    {
        gameOverScreen.SetActive(false);
        GameManager.main.StartNextRun();
    }
    public void BackToMenuButton()
    {
        gameOverScreen.SetActive(false);
        GameManager.main.ResetGame();
        Menu.main.ReturnToMenu();
    }

    IEnumerator RevealTravelLine()
    {
        yield return new WaitForSeconds(lineRevealTime);

        float timeElapsed = 0f;

        float dur1 = lineRevealTime / 3f;
        float dur2 = dur1 * 2f;

        while(timeElapsed < dur2)
        {
            underLine.localScale = Vector3.Lerp(Vector3.one, underlineFinishScale, timeElapsed / dur2);

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        underLine.localScale = underlineFinishScale;
        timeElapsed = 0f;

        while(timeElapsed < dur1)
        {
            float t = timeElapsed / dur1;

            trainLineScreen.localScale = new Vector3(1f, Mathf.Lerp(0f, 1f, t), 1f);
            gameOverText.anchoredPosition = Vector3.Lerp(gameOverTextStartPos, gameOverTextEndPos, t);

            if(t > 0.4f) { underLine.localScale = Vector3.zero; }

            timeElapsed += Time.deltaTime;
            yield return null;
        }
        trainLineScreen.localScale = Vector3.one;
        gameOverText.anchoredPosition = gameOverTextEndPos;

        float targetValue = RunLogger.main.SpinCount() / 50f;
        timeElapsed = 0f;

        while(timeElapsed < gameOverScreenDuration)
        {
            gameOverSlider.value = Mathf.Lerp(0f, targetValue, timeElapsed / gameOverScreenDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameOverSlider.value = targetValue;
        xMarker.gameObject.SetActive(true);
        if (InputManager.main.ActiveDevice == InputManager.InputDevice.Controller)
        {
            defaultGameOverControllerButton.GainFocus();
        }

    }


    #endregion

    #region Pause
    [Header("Pause")]
    [SerializeField] GameObject pauseScreen;
    [SerializeField] ControllerButton defaultPauseControllerButton;

    public void Paused()
    {
        pauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.UI);
        if (InputManager.main.ActiveDevice == InputManager.InputDevice.Controller)
        {
            defaultPauseControllerButton.GainFocus();
        }
    }

    public void Unpaused()
    {
        pauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Player.local.ReleaseControlOfCamera();
    }

    public void ResumeButton()
    {
        GameManager.main.UnpauseGame();
    }

    #endregion

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        SetupGameOverValues();
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.anyKeyDown) { waitingForInput = false; }
        }
    }

    public void SwitchToControllerInput()
    {
        if (postScreen.activeInHierarchy) { defaultPostControllerButton.GainFocus(); }
        if (gameOverScreen.activeInHierarchy) { defaultGameOverControllerButton.GainFocus(); }
        if (pauseScreen.activeInHierarchy) { defaultPauseControllerButton.GainFocus(); }
    }
}
