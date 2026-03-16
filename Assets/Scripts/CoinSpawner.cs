using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public static CoinSpawner main;

    [SerializeField] GameObject coinPrefab;

    [SerializeField] Transform[] spawnPoints;
    [SerializeField] Transform explosionCentre;
    [SerializeField] float minSpawnForce, maxSpawnForce;
    [SerializeField] float delayBetweenSpawn;
    List<GameObject> coins = new List<GameObject>();
    int toSpawn = 0;
    bool spawning = false;
    bool delaying = false;

    private void Awake()
    {
        main = this;
    }

    [ContextMenu("DebugSpawn20")]
    public void DebugSpawn()
    {
        SpawnCoins(20);
    }

    public void SpawnCoins(int amount)
    {
        if (spawning)
        {
            toSpawn += amount;
        }
        else
        {
            StartCoroutine(SpawnCoinsCoroutine(amount));
        }
    }

    public void SpawnCoinsAfterDelay(int amount)
    {
        toSpawn += amount;
        if (!delaying && !spawning)
        {
            StartCoroutine(SpawnDelay(1f));
        }
    }

    IEnumerator SpawnDelay(float delay)
    { 
        delaying = true;
        yield return new WaitForSeconds(delay);

        if (!spawning)
        {
            StartCoroutine(SpawnCoinsCoroutine(0));
        }
        
        delaying = false;
    }

    IEnumerator SpawnCoinsCoroutine(int amount)
    {
        spawning = true;
        toSpawn += amount;

        while (toSpawn > 0)
        {
            int wave = spawnPoints.Length;
            if(wave > toSpawn) { wave = toSpawn; }

            if(wave == spawnPoints.Length)
            {
                foreach(Transform t in spawnPoints)
                {
                    GameObject obj = Instantiate(coinPrefab);
                    obj.transform.position = t.position;
                    coins.Add(obj);
                    obj.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(minSpawnForce, maxSpawnForce), explosionCentre.position, 0f);
                }
            }
            else
            {
                List<Transform> list = new List<Transform>(spawnPoints);
                for(int i = 0; i < wave; i++)
                {
                    Transform t = list[Random.Range(0, list.Count)];
                    GameObject obj = Instantiate(coinPrefab);
                    obj.transform.position = t.position;
                    coins.Add(obj);
                    obj.GetComponent<Rigidbody>().AddExplosionForce(Random.Range(minSpawnForce, maxSpawnForce), explosionCentre.position, 0f);
                    list.Remove(t);
                }
            }


            toSpawn -= wave;
            if(toSpawn > 0) { yield return new WaitForSeconds(delayBetweenSpawn); }
        }

        spawning = false;
    }

    public GameObject[] CollectCoins()
    {
        GameObject[] array = coins.ToArray();
        coins.Clear();
        return array;
    }
}
