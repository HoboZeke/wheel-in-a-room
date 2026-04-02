using System.Collections;
using UnityEngine;

public class TrainCabinAnimator : MonoBehaviour
{
    [SerializeField] SkinnedMeshRenderer cabinMeshRenderer;
    [SerializeField] float blendshapeMaxValue;
    [SerializeField] Vector2[] animTimeAndValue;
    [SerializeField] float uncrushTime;

    [ContextMenu("Play Crush Animation")]
    public void PlayCrushAnimation()
    {
        StartCoroutine(AnimateCrush());
    }

    [ContextMenu("Reset to Rest")]
    public void ResetToRestAnimation()
    {
        AnimationFrame(0f);
    }

    public void StopCrushAnimation()
    {
        StopCoroutine(AnimateCrush());
        StartCoroutine(AnimatedUndoCrush());
    }

    IEnumerator AnimateCrush()
    {
        float timeElapsed = 0f;
        int crushStep = 0;
        float startValue = 0f;
        float endValue = 0f;
        float stepTime = 0f;

        while(crushStep < animTimeAndValue.Length)
        {
            stepTime = animTimeAndValue[crushStep].x;
            endValue = animTimeAndValue[crushStep].y;
            timeElapsed = 0f;

            while(timeElapsed < stepTime)
            {
                AnimationFrame(Mathf.Lerp(startValue, endValue, timeElapsed / stepTime));

                timeElapsed += Time.deltaTime;
                yield return null;
            }

            AnimationFrame(endValue);
            startValue = endValue;
            crushStep++;
        }

        Debug.Log("Crush Animation Done");
    }

    IEnumerator AnimatedUndoCrush()
    {
        float timeElapsed = 0f;
        float startValue = cabinMeshRenderer.GetBlendShapeWeight(0);
        float endValue = 0f;

        while (timeElapsed < uncrushTime)
        {

             AnimationFrame(Mathf.Lerp(startValue, endValue, timeElapsed / uncrushTime));

             timeElapsed += Time.deltaTime;
             yield return null;

        }
    }

    void AnimationFrame(float t)
    {
        t = t * blendshapeMaxValue;

        cabinMeshRenderer.SetBlendShapeWeight(0, t);
    }
}
