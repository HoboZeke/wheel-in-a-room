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
    [SerializeField] RectTransform globeImage;
    [SerializeField] Transform lineHolder;
    [SerializeField] GameObject lineSegPrefab;
    [SerializeField] float gameOverScreenDuration;
    Vector3 gameOverLineStart, gameoverStepSize;
    float minVariance, maxVariance;
    List<GameObject> line = new List<GameObject>();

    void ClearLine()
    {
        foreach(GameObject go in line) { Destroy(go); }
        line.Clear();
    }

    void SetupGameOverValues()
    {
        ClearLine();
        gameOverLineStart = globeImage.sizeDelta * 0.35f;
        gameOverLineStart = new Vector3(gameOverLineStart.x * -1f, gameOverLineStart.y, gameOverLineStart.z);
        gameoverStepSize = gameOverLineStart / 120f;
        gameoverStepSize *= -1f;
        minVariance = gameoverStepSize.x * 0.5f;
        maxVariance = gameoverStepSize.x * 1.5f;
    }

    public void GameOverUI()
    {
        ToggleCountDownUI(false);
        gameOverScreen.SetActive(true);
        StartCoroutine(PlotJourney());
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

    Vector3[] JourneyPositions()
    {
        List<Vector3> pos = new List<Vector3>() { gameOverLineStart };

        for(int i = 1; i <= RunLogger.main.SpinCount(); i++)
        {
            Vector3 v = pos[pos.Count - 1] + gameoverStepSize;
            v += new Vector3(Random.Range(minVariance, maxVariance), Random.Range(minVariance, maxVariance) * -1f, 1f);
            pos.Add(v);
        }

        return pos.ToArray();
    }

    IEnumerator PlotJourney()
    {
        Vector3[] plotPoint = JourneyPositions();
        int step = 0;
        float timeElapsed = 0f;

        while(timeElapsed < gameOverScreenDuration)
        {
            float t = timeElapsed / gameOverScreenDuration;
            int intT = Mathf.FloorToInt(t * plotPoint.Length);

            if (intT >= step)
            {
                int dif = intT - step;
                for(int i = 0; i <= dif; i++)
                {
                    step++;
                    GameObject lSeg = Instantiate(lineSegPrefab);
                    lSeg.transform.SetParent(lineHolder);
                    lSeg.transform.localPosition = plotPoint[step];
                    lSeg.transform.up = plotPoint[step + 1] - lSeg.transform.position;
                    lSeg.transform.localScale = new Vector2(lSeg.transform.localScale.x, Vector3.Distance(plotPoint[step], plotPoint[step + 1]));
                    line.Add(lSeg);
                }
            }

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        for (int i = 0; i < line.Count; i++)
        {
            GameObject lSeg = line[i];
            lSeg.transform.localPosition = plotPoint[i];
            lSeg.transform.up = plotPoint[i + 1] - lSeg.transform.position;
            lSeg.transform.localScale = new Vector2(lSeg.transform.localScale.x, Vector3.Distance(plotPoint[step], plotPoint[step + 1]));
        }
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


}
