using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour
{
    public static Menu main;

    [SerializeField] GameObject menuScreen;
    [SerializeField] Transform menuCamera;
    [SerializeField] Image gameViewCoverImage;
    [SerializeField] float fadeOutTime;
    [SerializeField] ControllerButton defaultControllerButton, defaultOptionsButton;
    enum ViewScreen { MenuScreen, Story, Options }
    ViewScreen currentScreen = ViewScreen.MenuScreen;

    [Header("Camera Movement Script")]
    [SerializeField] Vector3 cameraBasePos;
    [SerializeField] Vector3 cameraBaseRot;
    [SerializeField] Vector3[] cameraTrackPoints;
    [SerializeField] Vector3[] cameraRotationPoints;
    [SerializeField] float[] cameraTrackTimes;
    [SerializeField] Vector3 optionsCameraRot, optionsCameraPos;
    [SerializeField] Vector3 storyCameraRot;
    [SerializeField] float cameraRotationDuration;
    bool busy;


    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        if(InputManager.main.ActiveDevice == InputManager.InputDevice.Controller)
        {
            defaultControllerButton.GainFocus();
        }
    }

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.anyKeyDown) { waitingForInput = false; }
        }
    }

    public void StartGame()
    {
        if (busy) { return; }
        StartCoroutine(StartGameAnimation());
    }
    public void SwitchToControllerInput()
    {
        switch (currentScreen)
        {
            case ViewScreen.MenuScreen:
                defaultControllerButton.GainFocus();
                break;
            case ViewScreen.Options:
                defaultOptionsButton.GainFocus();
                break;

        }
    }

    [ContextMenu("ResetCamera")]
    public void ResetCamera()
    {
        menuCamera.localPosition = cameraBasePos;
        menuCamera.localEulerAngles = cameraBaseRot;
    }

    [ContextMenu("Go To One")]
    public void SetCameraToPosOneCamera()
    {
        menuCamera.localPosition = cameraTrackPoints[0];
        menuCamera.localEulerAngles = cameraRotationPoints[0];
    }

    [ContextMenu("AddCurrentCameraPosToAnimationTrack")]
    void LogCameraPos()
    {
        List<Vector3> pos = new List<Vector3>(cameraTrackPoints);
        List<Vector3> rots = new List<Vector3>(cameraRotationPoints);

        pos.Add(menuCamera.localPosition);
        rots.Add(menuCamera.localEulerAngles);

        cameraTrackPoints = pos.ToArray();
        cameraRotationPoints = rots.ToArray();
    }

    public bool InMenu() { return menuScreen.activeInHierarchy; }
    public void ReturnToMenu()
    {
        ResetCamera();
        menuScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.UI);
        currentScreen = ViewScreen.MenuScreen;
    }

    public void MoveCameraToOptions()
    {
        if (busy) { return; }
        StartCoroutine(MoveAndRotateCamera(cameraBasePos, optionsCameraPos, cameraBaseRot, optionsCameraRot, cameraRotationDuration));
        currentScreen = ViewScreen.Options;
    }

    public void MoveCameraFromOptions()
    {
        if (busy) { return; }
        StartCoroutine(MoveAndRotateCamera(optionsCameraPos, cameraBasePos, optionsCameraRot, cameraBaseRot, cameraRotationDuration));
        currentScreen = ViewScreen.MenuScreen;
    }

    public void MoveCameraToStory()
    {
        if (busy) { return; }
        StartCoroutine(RotateCamera(cameraBaseRot, storyCameraRot, cameraRotationDuration));
        SetupStory();
        currentScreen = ViewScreen.Story;
    }

    public void MoveCameraFromStory()
    {
        if (busy) { return; }
        rocketLaunching = false;
        StartCoroutine(RotateCamera(storyCameraRot, cameraBaseRot, cameraRotationDuration));
        currentScreen = ViewScreen.MenuScreen;
    }

    IEnumerator RotateCamera(Vector3 from, Vector3 to, float dur)
    {
        busy = true;

        float timeElapsed = 0f;

        while (timeElapsed < dur)
        {
            menuCamera.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(from), Quaternion.Euler(to), timeElapsed / dur);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        menuCamera.transform.localEulerAngles = to;
        busy = false;
    }

    IEnumerator MoveAndRotateCamera(Vector3 from, Vector3 to, Vector3 fromRot, Vector3 toRot, float dur)
    {
        busy = true;
        float timeElapsed = 0f;

        while (timeElapsed < dur)
        {
            menuCamera.transform.localPosition = Vector3.Lerp(from, to, timeElapsed / dur);
            menuCamera.transform.localRotation = Quaternion.Lerp(Quaternion.Euler(fromRot), Quaternion.Euler(toRot), timeElapsed / dur);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        menuCamera.transform.localPosition = to;
        menuCamera.transform.localEulerAngles = toRot;
        busy = false;
    }

    IEnumerator StartGameAnimation()
    {
        busy = true;
        Cursor.lockState = CursorLockMode.Locked;

        float timeElapsed = 0f;
        int step = 0;
        Vector3 startPos = cameraBasePos;
        Quaternion startrot = Quaternion.Euler(cameraBaseRot);

        while (step < cameraTrackPoints.Length)
        {
            float t = timeElapsed / cameraTrackTimes[step];

            menuCamera.localPosition = Vector3.Lerp(startPos, cameraTrackPoints[step], t);
            menuCamera.localRotation = Quaternion.Lerp(startrot, Quaternion.Euler(cameraRotationPoints[step]), t);

            yield return null;
            timeElapsed += Time.deltaTime;
            if(timeElapsed >= cameraTrackTimes[step])
            {
                startPos = cameraTrackPoints[step];
                startrot = Quaternion.Euler(cameraRotationPoints[step]);
                timeElapsed -= cameraTrackTimes[step];
                
                step++;
            }
        }

        gameViewCoverImage.color = Color.black;
        gameViewCoverImage.gameObject.SetActive(true);

        menuScreen.gameObject.SetActive(false);

        timeElapsed = 0f;

        while(timeElapsed < fadeOutTime)
        {
            gameViewCoverImage.color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), timeElapsed / fadeOutTime);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        gameViewCoverImage.gameObject.SetActive(false);
        AudioManager.main.SwitchToGameMusic();

        Player.local.ReleaseControlOfCamera();
        busy = false;

        GameManager.main.StartRun();
    }

    #region Story
    [Header("Story")]
    [SerializeField] TextMeshProUGUI storyText;
    [TextArea(3, 10)]
    [SerializeField] string[] storyStrings;
    [SerializeField] float[] storyTimes;
    [SerializeField] float storyStartDelay;
    [SerializeField] int moonTurnIndex, rocketLaunchIndex;
    [SerializeField] GameObject directionalLight;
    [SerializeField] Transform[] rockets;
    [SerializeField] ParticleSystem[] rocketPS;
    [SerializeField] Transform moon;
    [SerializeField] Light moonLight;
    [SerializeField] float rocketLaunchSpeed;
    [SerializeField] float lightIntensity, moonTurnDuration;
    bool waitingForInput;
    Coroutine storyCoroutine;
    bool rocketLaunching;
    Vector3[] rocketStartPos;

    void SetupStory()
    {
        storyText.text = "";
        if(storyCoroutine != null ) { StopCoroutine(storyCoroutine); }

        storyCoroutine = StartCoroutine(PlayStory(storyStartDelay + cameraRotationDuration));
        moonLight.intensity = lightIntensity;
    }

    IEnumerator PlayStory(float initialDelay)
    {
        yield return new WaitForSeconds(initialDelay);
        directionalLight.SetActive(false);

        float timeElapsed = 0f;
        storyText.text = "";

        for (int i = 0; i < storyStrings.Length; i++)
        {
            if(i == moonTurnIndex) { StartCoroutine(TurnMoon()); }
            if(i == rocketLaunchIndex) { StartCoroutine(LaunchRockets()); }

            while (timeElapsed < storyTimes[i])
            {
                int limit = Mathf.RoundToInt(storyStrings[i].Length * (timeElapsed / storyTimes[i]));
                storyText.text = storyStrings[i].Substring(0, limit);

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            storyText.text = storyStrings[i];
            timeElapsed = 0f;

            yield return StartCoroutine(WaitForInput());

        }

        directionalLight.SetActive(true);

        rocketLaunching = false;
        MoveCameraFromStory();
    }

    IEnumerator TurnMoon()
    {
        float timeElapsed = 0f;

        while (timeElapsed < moonTurnDuration)
        {
            moonLight.intensity = Mathf.Lerp(lightIntensity, 0f, timeElapsed / moonTurnDuration);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        moonLight.intensity = 0f;
    }

    IEnumerator LaunchRockets()
    {
        rocketLaunching = true;

        rocketStartPos = new Vector3[rockets.Length];
        for(int i = 0; i < rockets.Length; i++) { rocketStartPos[i] = rockets[i].position; }

        foreach(ParticleSystem ps in rocketPS) { ps.Play(); }

        while (rocketLaunching)
        {
            foreach(Transform t in rockets) { t.localPosition += (rocketLaunchSpeed * Time.deltaTime) * t.forward; }

            yield return null;
        }

        for(int i = 0; i < rockets.Length; i++) { rockets[i].position = rocketStartPos[i]; }
        foreach (ParticleSystem ps in rocketPS) { ps.Stop(); }
    }

    IEnumerator WaitForInput()
    {
        waitingForInput = true;
        while (waitingForInput) { yield return null; }
    }

#endregion
}
