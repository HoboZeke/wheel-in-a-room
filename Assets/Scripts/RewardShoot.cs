using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RewardShoot : MonoBehaviour
{
    public static RewardShoot main;

    [SerializeField] GameObject fuelSpherePrefab;
    List<GameObject> fuel = new List<GameObject>();

    [SerializeField] TextMeshProUGUI contextValueText;
    [SerializeField] Vector3[] shootPath, shootPathScale;
    [SerializeField] float spawnAnimDuration;
    [SerializeField] float delayBetweenSpawn;
    int queue;
    bool queuedSpawns;
    bool leverBusy;

    private void Awake()
    {
        main = this;
    }

    public void SpawnFuelReward(int amount)
    {
        queue += amount;
        if(!queuedSpawns)
        {
            StartCoroutine(SpawnQueue());
        }
    }

    public void DepositCoalIntoFurnance()
    {
        if (leverBusy) { return; }

        if(fuel.Count >= Furnance.main.NextInputAmount())
        {
            StartCoroutine(DepositCoalInFurnance(Furnance.main.NextInputAmount()));
        }
    }

    void UpdateContents()
    {
        if(fuel.Count == 0) { contextValueText.text = "Empty"; }
        else
        {
            contextValueText.text = "Fuel x" + fuel.Count.ToString();
        }
    }

    GameObject ClosestFuelToFurnanceShoot()
    {
        float closestValue = fuel[0].transform.localPosition.x + (fuel[0].transform.localPosition.y * -1);
        GameObject closestFuel = fuel[0];

        for (int i = 1; i < fuel.Count; i++)
        {
            if(closestValue < fuel[i].transform.localPosition.x + (fuel[i].transform.localPosition.y * -1))
            {
                closestValue = fuel[i].transform.localPosition.x + (fuel[i].transform.localPosition.y * -1);
                closestFuel = fuel[i];
            }
        }

        return closestFuel;
    }

    IEnumerator SpawnQueue()
    {
        queuedSpawns = true;
        while(queue > 0)
        {
            GameObject obj = Instantiate(fuelSpherePrefab);
            fuel.Add(obj);
            UpdateContents();
            StartCoroutine(DepositObjInShoot(obj.transform, spawnAnimDuration));
            yield return new WaitForSeconds(delayBetweenSpawn);
            queue--;
        }
        queuedSpawns = false;
    }

    IEnumerator DepositObjInShoot(Transform t, float dur)
    {
        float timeElapsed = 0f;
        t.SetParent(transform);
        t.localPosition = shootPath[0];
        t.localScale = shootPathScale[0];

        float segmentDur = dur / shootPath.Length; 

        while (timeElapsed < dur)
        {
            float time = timeElapsed / dur;
            int i = Mathf.FloorToInt(time * (shootPath.Length-1));
            float segmentT = (time - (segmentDur * i)) / segmentDur;

            t.localPosition = Vector3.Lerp(shootPath[i], shootPath[i + 1], segmentT);
            t.localScale = Vector3.Lerp(shootPathScale[i], shootPathScale[i + 1], segmentT);

            timeElapsed += Time.deltaTime;
            yield return null;
        }

        t.localPosition = shootPath[shootPath.Length - 1];
        t.localScale = shootPathScale[shootPathScale.Length - 1];

        if(t.GetComponent<Rigidbody>() != null)
        {
            t.GetComponent<Rigidbody>().isKinematic = false;
        }
    }

    IEnumerator DepositCoalInFurnance(int amountToDeposit)
    {
        leverBusy = true;
        float dur = 0.4f;

        while(amountToDeposit > 0)
        {
            GameObject f = ClosestFuelToFurnanceShoot();
            fuel.Remove(f);
            UpdateContents();
            Destroy(f);
            Furnance.main.AddCoal(1);
            amountToDeposit--;

            yield return new WaitForSeconds(dur);
        }

        leverBusy = false;
    }
}
