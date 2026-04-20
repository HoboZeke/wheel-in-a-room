using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class CanaryStory : MonoBehaviour
{
    [SerializeField] Vector3 canaryFocusCameraPos;
    [SerializeField] Vector3 canaryFocueCameraRot;
    [SerializeField] Transform cameraFocusPoint;
    [SerializeField] GameObject storyScreen;
    [SerializeField] TextMeshProUGUI storyText;
    [SerializeField] TextMeshProUGUI storyContinueIndicator;
    [SerializeField] float continueIndicatorTickDur;
    [SerializeField] GameObject canaryTrinket;
    [SerializeField] Transform storyFocalPoint;
    Vector3 focalPointBasePos;

    [Header("Canary Choice")]
    [SerializeField] GameObject canaryChoiceScreen;
    [SerializeField] TextMeshProUGUI canaryChoiceTitle, canaryChoiceDesc, canaryChoiceInstruction;
    [SerializeField] Button canaryChoiceA, canaryChoiceB, canaryChoiceC;
    [SerializeField] TextMeshProUGUI choiceATitle, choiceBTitle, choiceCTitle;
    [SerializeField] TextMeshProUGUI choiceADesc, choiceBDesc, choiceCDesc;
    [SerializeField] TrinketProfile trinkA, trinkB, trinkC;
    [SerializeField] string[] choiceTitles, choiceDescriptions, choiceInstructions;

    [Header("Tutorial")]
    [SerializeField] StoryBeat[] tutorialBeats;
    [SerializeField] float cameraMoveDuration, storyTextDuration;

    [Header("Stories")]
    [SerializeField] StoryBeat[] runOneStory;
    [SerializeField] StoryBeat[] runTwoStory, runThreeStory;

    private void Awake()
    {
        focalPointBasePos = storyFocalPoint.position;
    }

    public void OnStartRun()
    {
        canaryTrinket.SetActive(false);
        storyFocalPoint.position = focalPointBasePos;

        switch (RunLogger.main.RunCount())
        {
            case 1:
                StartCoroutine(PlayStory(tutorialBeats));
                break;
            case 2:
                StartCoroutine(PlayStory(runOneStory));
                break;
            case 3:
                StartCoroutine(PlayStory(runTwoStory));
                break;
            case 4:
                StartCoroutine(PlayStory(runThreeStory));
                break;
            default:
                ShowCanaryChoice();
                break;
        }
    }

    void ShowCanaryChoice()
    {
        InputManager.main.SetBusy(true);
        TrinketManager.main.RefillCanaryPool();

        trinkA = TrinketManager.main.PullFromCanaryPool();
        trinkB = TrinketManager.main.PullFromCanaryPool();
        trinkC = TrinketManager.main.PullFromCanaryPool();

        SetupCanaryTrinketChoiceUI(trinkA, choiceATitle, choiceADesc, canaryChoiceA);
        SetupCanaryTrinketChoiceUI(trinkB, choiceBTitle, choiceBDesc, canaryChoiceB);
        SetupCanaryTrinketChoiceUI(trinkC, choiceCTitle, choiceCDesc, canaryChoiceC);

        int i = Random.Range(0, choiceTitles.Length);

        canaryChoiceTitle.text = choiceTitles[i]; 
        
        if (i >= choiceDescriptions.Length) { i = choiceDescriptions.Length - 1; }
        canaryChoiceDesc.text = choiceDescriptions[i];

        if(i >= choiceInstructions.Length) { i = choiceInstructions.Length - 1; }
        canaryChoiceInstruction.text = choiceInstructions[i];

        canaryChoiceScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    void SetupCanaryTrinketChoiceUI(TrinketProfile t, TextMeshProUGUI title, TextMeshProUGUI desc, Button button)
    {
        title.text = t.TrinketName;
        desc.text = t.TrinketDescription;
        button.interactable = true;
    }

    public void ChooseCanaryTrinket(int slot)
    {
        storyScreen.SetActive(false);
        canaryChoiceScreen.SetActive(false);

        TrinketProfile t = trinkA;
        if(slot == 2) { t = trinkB; }
        else if(slot == 3) { t = trinkC; }

        canaryTrinket.GetComponent<Trinket>().Setup(t);
        canaryTrinket.GetComponent<TrinketObject>().SetupTrinket(canaryTrinket.GetComponent<Trinket>());

        canaryTrinket.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;
        Player.local.ReleaseControlOfCamera();
        InputManager.main.SetBusy(false);
    }

    IEnumerator PlayStory(StoryBeat[] beats)
    {
        InputManager.main.SetBusy(true);

        storyText.text = "";
        storyContinueIndicator.gameObject.SetActive(false);
        storyScreen.SetActive(true);
        canaryChoiceScreen.SetActive(false);
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.Story);
        Player.local.ForceLookAt(storyFocalPoint);

        for (int i = 0; i < beats.Length; i++) 
        {
            yield return StartCoroutine(PlayBeat(beats[i]));
        }

        Player.local.ReleaseLookAt();
        InputManager.main.SetBusy(false);
        Player.local.MovePlayerToPos(Vector3.zero, Vector3.zero);
        ShowCanaryChoice();
    }

    IEnumerator PlayBeat(StoryBeat beat)
    {
        float timeElapsed = 0f;
        storyText.text = "";
        Vector3 startPos = Player.local.GetPosition();
        Vector3 startRot = Player.local.GetRotation();
        Vector3 focalStart = storyFocalPoint.position;

        if(Vector3.Distance(startPos, beat.cameraPos) < 0.2f)
        {
            timeElapsed = cameraMoveDuration;
        }

        while(timeElapsed < cameraMoveDuration)
        {
            float t = timeElapsed / cameraMoveDuration;

            Player.local.MovePlayerToPos(Vector3.Lerp(startPos, beat.cameraPos, t), startRot);
            storyFocalPoint.position = Vector3.Lerp(focalStart, beat.cameraFocalPos, t);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        timeElapsed = 0f;
        Player.local.MovePlayerToPos(beat.cameraPos, startRot);
        storyFocalPoint.position = beat.cameraFocalPos;

        char[] chars = beat.storyText.ToCharArray();

        while(timeElapsed < storyTextDuration)
        {
            int length = Mathf.FloorToInt(Mathf.Lerp(0, chars.Length, timeElapsed / storyTextDuration));
            storyText.text = new string(chars, 0, length);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        storyText.text = new string(chars);

        if (beat.autoComplete) { yield return new WaitForSeconds(0.5f); }
        else { yield return StartCoroutine(WaitForInput()); }


        storyText.text = "";
    }

    bool waitingForInput = false;

    private void Update()
    {
        if (waitingForInput)
        {
            if (Input.anyKeyDown) { InputRecieved(); }
        }
    }

    public void InputRecieved() { waitingForInput = false; }

    IEnumerator WaitForInput()
    {
        waitingForInput = true;
        float tickTime = 0f;
        bool tick = false;
        storyContinueIndicator.gameObject.SetActive(true);

        while (waitingForInput) 
        { 
            tickTime += Time.deltaTime;

            yield return null; 

            if(tickTime > continueIndicatorTickDur)
            {
                tickTime = 0f;
                storyContinueIndicator.gameObject.SetActive(tick);
                tick = !tick;
            }
        }
    }
}

[Serializable]
public class StoryBeat
{
    public Vector3 cameraPos;
    public Vector3 cameraFocalPos;
    public bool autoComplete;
    [TextArea(3, 10)]
    public string storyText;
}
