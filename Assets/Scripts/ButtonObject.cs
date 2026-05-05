using UnityEngine;
using UnityEngine.Events;

public class ButtonObject : MonoBehaviour
{
    [SerializeField] UnityEvent OnClick;
    [SerializeField] UnityEvent OnEnter, OnExit;

    public void Activate()
    {
        OnClick?.Invoke();
        AudioManager.main.PlayUIButtonSFX();
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
