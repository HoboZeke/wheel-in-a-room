using TMPro;
using UnityEngine;

public class UIPostOptionPanel : MonoBehaviour
{
    [SerializeField] int panelIndex;

    [SerializeField] TextMeshProUGUI panelTitleText, panelDescriptionText;

    public void SetIndex(int i) {  panelIndex = i; }

    public void OnButtonPressed()
    {
        UIController.main.SelectPostOption(panelIndex);
    }

    public void SetupPanel(string title, string description)
    {
        panelTitleText.text = title;
        panelDescriptionText.text = description;
    }
}
