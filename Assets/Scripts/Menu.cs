using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public static Menu main;

    [SerializeField] GameObject menuScreen;
    [SerializeField] Transform menuCamera;
    [SerializeField] Image gameViewCoverImage;
    [SerializeField] float fadeOutTime;

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
    }

    public void StartGame()
    {
        if (busy) { return; }
        StartCoroutine(StartGameAnimation());
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

    public void ReturnToMenu()
    {
        ResetCamera();
        menuScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.UI);
    }

    public void MoveCameraToOptions()
    {
        if (busy) { return; }
        StartCoroutine(MoveAndRotateCamera(cameraBasePos, optionsCameraPos, cameraBaseRot, optionsCameraRot, cameraRotationDuration));
    }

    public void MoveCameraFromOptions()
    {
        if (busy) { return; }
        StartCoroutine(MoveAndRotateCamera(optionsCameraPos, cameraBasePos, optionsCameraRot, cameraBaseRot, cameraRotationDuration));
    }

    public void MoveCameraToStory()
    {
        if (busy) { return; }
        StartCoroutine(RotateCamera(cameraBaseRot, storyCameraRot, cameraRotationDuration));
    }

    public void MoveCameraFromStory()
    {
        if (busy) { return; }
        StartCoroutine(RotateCamera(storyCameraRot, cameraBaseRot, cameraRotationDuration));
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
}
