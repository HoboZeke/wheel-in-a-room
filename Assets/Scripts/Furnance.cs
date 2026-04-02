using System.Collections;
using TMPro;
using UnityEngine;

public class Furnance : MonoBehaviour
{
    public static Furnance main;

    [SerializeField] PostBox postBox;
    [SerializeField] GameObject fireObject;
    [SerializeField] GameObject[] coalObjects;
    [SerializeField] TextMeshProUGUI coalCountText, spinsCountText, coalNeededText;
    [SerializeField] int coalRequired;
    [SerializeField] int spinsGenerated;
    int heldCoal;
    [SerializeField] float[] coalReqMulti;
    bool busy;

    [Header("Soot")]
    [SerializeField] ParticleSystem sootPS;
    [SerializeField] float sootAnimationDuration;



    private void Awake()
    {
        main = this;    
    }

    private void Start()
    {
        UpdateCoalVisuals();
    }

    int CoalRequiredForFiring()
    {
        return Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(spinsGenerated));
    }

    int SpinsGainedFromFiring()
    {
        int cur = spinsGenerated;
        int coalCount = heldCoal;

        while (coalCount >= Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(cur)))
        {
            coalCount -= Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(cur));
            cur++;
        }

        return (cur - spinsGenerated) * 3;
    }

    public int NextInputAmount()
    {
        int cur = spinsGenerated;
        int coalCount = heldCoal;

        while(coalCount >= Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(cur)))
        {
            coalCount -= Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(cur));
            cur++;
        }

        return Mathf.FloorToInt(coalRequired * CoalRequiredMultiplier(cur)) - coalCount;
    }

    float CoalRequiredMultiplier(int prevFires)
    {
        if (prevFires < coalReqMulti.Length) {
            Debug.Log("Returning Coal Req Multi " + coalReqMulti[prevFires]);
            return coalReqMulti[prevFires]; }
        else { return coalReqMulti[coalReqMulti.Length - 1] * (prevFires - coalReqMulti.Length + 2); }
    }

    public void AddCoal(int amount)
    {
        heldCoal += amount;
        UpdateCoalVisuals();
    }

    void UpdateCoalVisuals()
    {
        for (int i = 0; i < coalObjects.Length; i++)
        {
            coalObjects[i].SetActive(i < heldCoal);
        }
        
        coalCountText.text = heldCoal.ToString();
        spinsCountText.text = SpinsGainedFromFiring().ToString();
        coalNeededText.text = NextInputAmount().ToString();
    }

    public void FireCoal()
    {
        if(busy) return;

        if(heldCoal >= CoalRequiredForFiring())
        {
            StartCoroutine(Firing());
        }
    }

    IEnumerator Firing()
    {
        busy = true;

        int fireCount = 0;

        while(heldCoal >= CoalRequiredForFiring())
        {
            heldCoal -= CoalRequiredForFiring();
            spinsGenerated++;
            fireCount++;
            EnvironmentManager.main.MultiplyTunnelSpeed(1.5f);
        }

        heldCoal = 0;
        fireObject.gameObject.SetActive(true);
        ProgressTracker.main.AddSpins(fireCount * 3);

        yield return new WaitForSeconds(3f);
        
        fireObject.gameObject.SetActive(false);
        UpdateCoalVisuals();

        StartCoroutine(ExpelSoot());

        postBox.PopulatePost();

        busy = false;
    }

    IEnumerator ExpelSoot()
    {
        sootPS.gameObject.SetActive(true);
        sootPS.Play();

        yield return new WaitForSeconds(sootAnimationDuration);

        Wheel.main.AddToSegment(1, WheelSegment.SegmentColour.Soot);
    }
}
