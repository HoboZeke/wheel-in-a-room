using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] UnityEvent OnEnter, OnExit;

    public void Activate()
    {
        OnClick?.Invoke();
    }

    private void OnMouseDown()
    {
        Activate();
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
