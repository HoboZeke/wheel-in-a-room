using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CoinScoop : MonoBehaviour
{

    [SerializeField] Transform scoopEntrance;
    [SerializeField] int coinCount;
    [SerializeField] TextMeshProUGUI coinCountText;
    [SerializeField] float coinSuckSpeed;
    bool sucking = false;

    public void SuckUpCoins()
    {
        if(sucking) { return; }

        StartCoroutine(SuckUpCoinsCoroutine(CoinSpawner.main.CollectCoins()));
    }

    IEnumerator SuckUpCoinsCoroutine(GameObject[] coins)
    {
        sucking = true;
        List<GameObject> list = new List<GameObject>(coins);

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

        sucking = false;
    }

    void IncreaseCoinCount(int amount)
    {
        coinCount += amount;
        coinCountText.text = amount.ToString();
    }
}
