using TMPro;
using UnityEngine;

public class ProgressTracker : MonoBehaviour
{
    public static ProgressTracker main;

    [SerializeField] Transform trackerLine;
    [SerializeField] Transform train, monster;
    [SerializeField] Transform[] markers;
    [SerializeField] TextMeshProUGUI spinsLeftText;
    [SerializeField] int spinsLeft;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        SetSpins(3);
    }

    public void SetSpins(int amount)
    {
        spinsLeft = amount;
        SetupTracker();
    }

    public void AddSpins(int amount)
    {
        spinsLeft += amount;
        SetupTracker();
    }

    public void UseSpin()
    {
        spinsLeft--;
        SetupTracker();
    }

    public int SpinsLeft() { return spinsLeft; }

    void SetupTracker()
    {
        trackerLine.localScale = new Vector3(spinsLeft * 0.5f, 1f, 1f);
        train.localPosition = new Vector3(trackerLine.localScale.x / 2f, 0f, 0f);
        monster.localPosition = new Vector3(-trackerLine.localScale.x / 2f, 0f, 0f);

        for (int i = 0; i < markers.Length; i++)
        {
            if (i < spinsLeft)
            {
                markers[i].localPosition = Vector3.Lerp(monster.localPosition, train.localPosition, i / (float)spinsLeft) + new Vector3(0f, 0.01f, 0f);
            }
            else
            {
                markers[i].localPosition = train.localPosition + new Vector3(0f, 0.01f, 0f);
            }
        }

        spinsLeftText.text = "Spins: " + spinsLeft;
    }
}
