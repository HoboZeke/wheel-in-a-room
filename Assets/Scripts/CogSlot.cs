using System.Collections;
using UnityEngine;

public class CogSlot : Interactable
{
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Transform cogA, cogB, cogC;
    [SerializeField] Vector3 baseEulerA, baseEulerB, baseEulerC;
    [SerializeField] Transform riser;
    [SerializeField] Vector3 riserBasePos, riserEndPos;
    [SerializeField] float cogSpinSpeed;
    [SerializeField] float riserSpeed;
    [SerializeField] float cogRestRotDifference;
    [SerializeField] BoxCollider keyCollider;
    [SerializeField] Rigidbody keyRB;
    bool cogPlaced;
    bool spinning;

    public void SpinCogs()
    {
        StartCoroutine(SpinAnimation());
    }

    public void EndSpin()
    {
        spinning = false;
    }

    public override void Interact()
    {
        if (PlayerInventory.main.HasCog())
        {
            cogB.gameObject.SetActive(true);
            cogPlaced = true;
            PlayerInventory.main.RemoveCogFromInv();
            boxCollider.enabled = false;
        }
    }

    void RiserAtTop()
    {
        keyCollider.enabled = true;
        keyRB.isKinematic = false;
    }

    IEnumerator SpinAnimation()
    {
        spinning = true;

        while (spinning)
        {
            cogA.Rotate(Vector3.back * cogSpinSpeed * Time.deltaTime);
            if (cogPlaced)
            {
                cogB.Rotate(Vector3.forward * cogSpinSpeed * Time.deltaTime);
                cogC.Rotate(Vector3.back * cogSpinSpeed * Time.deltaTime);

                if (riser.transform.localPosition != riserEndPos)
                {
                    riser.transform.localPosition += Vector3.up * riserSpeed * Time.deltaTime;
                    if (riser.transform.localPosition.y > riserBasePos.y)
                    {
                        riser.transform.position = riserEndPos;
                        RiserAtTop();
                    }
                }
            }

            yield return null;
        }

        while (Mathf.Abs(cogA.localEulerAngles.z - baseEulerA.z) > cogRestRotDifference)
        {
            cogA.Rotate(Vector3.back * (cogSpinSpeed/2) * Time.deltaTime);
            if (cogPlaced)
            {
                cogB.Rotate(Vector3.forward * (cogSpinSpeed/2) * Time.deltaTime);
                cogC.Rotate(Vector3.back * (cogSpinSpeed/2) * Time.deltaTime);
            }
        }

        cogA.localEulerAngles = baseEulerA;
        cogB.localEulerAngles = baseEulerB;
        cogC.localEulerAngles = baseEulerC;
    }
}
