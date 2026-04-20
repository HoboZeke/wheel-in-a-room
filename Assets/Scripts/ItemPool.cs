using UnityEngine;

public class ItemPool : MonoBehaviour
{
    public static ItemPool main;

    [SerializeField] InventoryItem[] inventoryItems;
    [SerializeField] Vector3 poolLoc;
    Vector3[] itemBasePos;
    Vector3[] itemBaseRot;

    private void Awake()
    {
        main = this;
    }

    private void Start()
    {
        itemBasePos = new Vector3[inventoryItems.Length];
        itemBaseRot = new Vector3[inventoryItems.Length];

        for (int i = 0; i < inventoryItems.Length; i++)
        {
            itemBasePos[i] = inventoryItems[i].transform.position;
            itemBaseRot[i] = inventoryItems[i].transform.localEulerAngles;
        }
    }

    public void ResetInventoryItems()
    {
        for(int i = 0; i < inventoryItems.Length; i++)
        {
            inventoryItems[i].transform.localPosition = itemBasePos[i];
            inventoryItems[i].transform.localEulerAngles = itemBaseRot[i];
        }
    }

    public void RemoveItem(InventoryItem item)
    {
        item.transform.position = poolLoc;
    }
}
