using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] UnityEvent OnEnter, OnExit;

    private void OnMouseDown()
    {
        OnClick?.Invoke();
    }

    private void OnMouseEnter()
    {
        OnEnter?.Invoke();
    }

    private void OnMouseExit()
    {
        OnExit?.Invoke();
    }

}
