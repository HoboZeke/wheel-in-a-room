using TMPro;
using UnityEngine;

public class DebugUI : MonoBehaviour
{
    public static DebugUI main;

    [SerializeField] TextMeshProUGUI debugLabel;
    [SerializeField] bool showDebug;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        if(!showDebug) { gameObject.SetActive(false); }
    }

    public void DebugLabel(string text) { debugLabel.text = text; }
}
