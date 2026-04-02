using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController main;

    [Header("Post")]
    [SerializeField] PostBox postBox;
    [SerializeField] GameObject postScreen;
    [SerializeField] TextMeshProUGUI postTitleText, postFluffText, postMemoText;
    [SerializeField] UIPostOptionPanel[] postOptionsPanels;

    [Header("Countdown")]
    [SerializeField] GameObject countDownScreen;
    [SerializeField] TextMeshProUGUI countDownText;
    [SerializeField] Image countDownBackground;

    private void Awake()
    {
        main = this;
    }

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
}
