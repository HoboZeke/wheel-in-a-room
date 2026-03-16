using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScoop : MonoBehaviour
{
    public static CoinScoop main;

    [SerializeField] Transform scoopEntrance;
    [SerializeField] int coinCount;
    [SerializeField] TextMeshProUGUI coinCountText;
    [SerializeField] float coinSuckSpeed;
    [SerializeField] Transform fill;
    [SerializeField] int fillMax;
    [SerializeField] float fillYScaleMax;
    bool sucking = false;

    private void Awake()
    {
        main = this;
    }

    public void SuckUpCoins()
    {
        if(sucking) { return; }

        Debug.Log("Begin suck!");

        StartCoroutine(SuckUpCoinsCoroutine(CoinSpawner.main.CollectCoins()));
    }

    IEnumerator SuckUpCoinsCoroutine(GameObject[] coins)
    {
        sucking = true;
        List<GameObject> list = new List<GameObject>(coins);
        Debug.Log("Sucking " + coins.Length + " coins");

        foreach (GameObject co in list)
        {
            co.GetComponent<Rigidbody>().isKinematic = true;
        }

        while (list.Count > 0)
        {
            float speed = coinSuckSpeed * Time.deltaTime;
            for(int i = list.Count - 1; i >= 0; i--) 
            {
                GameObject coin = list[i];
                coin.transform.position = Vector3.MoveTowards(coin.transform.position, scoopEntrance.position, speed);
                if (coin.transform.position == scoopEntrance.position)
                {
                    Destroy(coin);
                    IncreaseCoinCount(1);
                    list.RemoveAt(i);
                }
            }

            yield return null;
        }

        Debug.Log("End Suck!");
        sucking = false;
    }

    void IncreaseCoinCount(int amount)
    {
        coinCount += amount;
        coinCountText.text = coinCount.ToString();
        UpdateFill();
    }

    public bool CanAfford(int coinCheck) { return coinCheck >= coinCount; }
    public bool SpendCoin(int amount)
    {
        if(amount > coinCount) { return false; }
        coinCount -= amount;
        coinCountText.text = coinCount.ToString();
        UpdateFill();
        return true;
    }

    void UpdateFill()
    {
        if(fillMax < coinCount) { fillMax = coinCount; }

        float y = ((float)coinCount / (float)fillMax) * fillYScaleMax;
        fill.localScale = new Vector3(fill.localScale.x, y, fill.localScale.z);
    }
}
