using System.Collections;
using UnityEngine;

public class RewardShoot : MonoBehaviour
{
    public static RewardShoot main;

    [SerializeField] GameObject fuelSpherePrefab;

    [SerializeField] Vector3[] shootPath, shootPathScale;
    [SerializeField] float spawnAnimDuration;
    [SerializeField] float delayBetweenSpawn;
    int queue;
    bool queuedSpawns;

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

    IEnumerator SpawnQueue()
    {
        queuedSpawns = true;
        while(queue > 0)
        {
            GameObject obj = Instantiate(fuelSpherePrefab);
            StartCoroutine(DepositObjInShoot(obj.transform, spawnAnimDuration));
            yield return new WaitForSeconds(delayBetweenSpawn);
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
            int i = Mathf.FloorToInt(time * shootPath.Length);
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
}
