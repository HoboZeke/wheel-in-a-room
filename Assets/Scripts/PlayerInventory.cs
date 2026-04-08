using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public static PlayerInventory main;

    [SerializeField] Transform cog, key, hammer;
    [SerializeField] List<Transform> inInventory = new List<Transform>();
    [SerializeField] Vector3 restPos;
    [SerializeField] Vector3 inventoryPos, inventorySpacer;
    [SerializeField] float inventoryMoveSpeed;
    [SerializeField] Vector3 inventorySpin;

    private void Awake()
    {
        main = this;
    }

    private void Update()
    {
        if(cog.gameObject.activeInHierarchy) cog.Rotate(inventorySpin * Time.deltaTime);
        if(key.gameObject.activeInHierarchy) key.Rotate(inventorySpin * Time.deltaTime);
        if (key.gameObject.activeInHierarchy) hammer.Rotate(inventorySpin * Time.deltaTime);
    }

    public bool HasCog() { return inInventory.Contains(cog); }
    public bool HasKey() { return inInventory.Contains(key); }
    public bool HasHammer() { return inInventory.Contains(hammer); }

    public void AddCogToInv() { StartCoroutine(MoveItemIntoInventory(cog)); }
    public void AddKeyToInv() { StartCoroutine(MoveItemIntoInventory(key)); }
    public void AddHammerToInv() { StartCoroutine (MoveItemIntoInventory(hammer)); }
    public void RemoveCogFromInv() { inInventory.Remove(cog); cog.gameObject.SetActive(false); }
    public void RemoveKeyFromInv() { inInventory.Remove(key); key.gameObject.SetActive(false); }
    public void RemoveHammerFromInv() { inInventory.Remove(hammer); hammer.gameObject.SetActive(false); }

    IEnumerator MoveItemIntoInventory(Transform t)
    {
        Vector3 targetPos = inventoryPos + (inventorySpacer * inInventory.Count);
        inInventory.Add(t);

        while(t.localPosition != targetPos)
        {
            t.localPosition = Vector3.MoveTowards(t.localPosition, targetPos, inventoryMoveSpeed * Time.deltaTime);

            yield return null;
        }
    }
}


