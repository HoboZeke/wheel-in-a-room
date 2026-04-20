using UnityEngine;

public class UIFocusCamera : MonoBehaviour
{
    [SerializeField] Vector3 spinEuler;

    [SerializeField] GameObject miniWheel;

    GameObject[] goArray;

    private void Start()
    {
        goArray = new GameObject[]
        {
            miniWheel
        };
    }

    private void Update()
    {
        for (int i = 0; i < goArray.Length; i++)
        {
            if (goArray[i].activeInHierarchy)
            {
                goArray[i].transform.Rotate(spinEuler * Time.deltaTime);
            }
        }
    }

    public void ToggleMiniWheel(bool toggle) { miniWheel.SetActive(toggle); }

    public void TurnOffAll() { foreach (GameObject go in goArray) { go.SetActive(false); } }
}
