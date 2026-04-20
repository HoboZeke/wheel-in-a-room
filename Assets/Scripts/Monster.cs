using System;
using System.Collections;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Transform monsterTransform;
    [SerializeField] Vector3 farthestPoint, closestPoint;
    [SerializeField] int maxDistanceSteps;
    [SerializeField] float moveSpeed;
    [SerializeField] Vector3 tiltDownEuler, tiltUpEuler;
    [SerializeField] Vector3 drawBackOffset, drawForwardOffset;
    [SerializeField] float animationDuration;
    Vector3 offset;
    Vector3 monsterPos;

    private void Start()
    {
        MoveMonster();
        ProgressTracker.main.OnSpinCountUpdate += MoveMonster;
        StartCoroutine(MonsterAnimation());
    }

    private void Update()
    {
        if(!MonsterOnItsSpot())
        {
            monsterTransform.localPosition = Vector3.MoveTowards(monsterTransform.localPosition, MonsterPos(), moveSpeed);
        }
    }

    bool MonsterOnItsSpot()
    {
        return monsterTransform.localPosition - offset == monsterPos;
    }

    void MoveMonster(object sender = null, EventArgs args = null)
    {
        monsterPos = Vector3.Lerp(closestPoint, farthestPoint, (float)ProgressTracker.main.SpinsLeft() / (float)maxDistanceSteps);
    }

    Vector3 MonsterPos() { return monsterPos + offset; }

    IEnumerator MonsterAnimation()
    {
        float timeElapsed = 0f;
        bool forward = true;

        while (gameObject.activeInHierarchy)
        {
            if (forward)
            {
                offset = Vector3.Lerp(drawBackOffset, drawForwardOffset, timeElapsed / animationDuration);
                monsterTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(tiltDownEuler), Quaternion.Euler(tiltUpEuler), timeElapsed / animationDuration);
            }
            else
            {
                offset = Vector3.Lerp(drawForwardOffset, drawBackOffset, timeElapsed / animationDuration);
                monsterTransform.localRotation = Quaternion.Lerp(Quaternion.Euler(tiltUpEuler), Quaternion.Euler(tiltDownEuler), timeElapsed / animationDuration);

            }

            timeElapsed += Time.deltaTime;
            yield return null;

            if(timeElapsed >= animationDuration)
            {
                forward = !forward;
                timeElapsed -= animationDuration;
            }
        }



    }

}
