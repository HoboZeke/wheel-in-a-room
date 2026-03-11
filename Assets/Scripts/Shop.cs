using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : Interactable
{
    [SerializeField] ShopSlot[] shopSlots;
    ShopSlot activeSlot;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] Vector3 shopViewPos, shopViewRot;
    [SerializeField] BoxCollider boxCollider;
    [Header("Tooltip")]
    [SerializeField] Transform tooltipBox;
    [SerializeField] TextMeshProUGUI tooltipTitle, tooltipDesc, tooltipType;
    [SerializeField] Button tooltipBuyButton;
    bool focused;

    private void Start()
    {
        PopulateShop();
        SelectActiveShopSlot(null);
    }

    public void PopulateShop()
    {
        for (int i = 0; i < shopSlots.Length; i++)
        {
            shopSlots[i].PutItemInSlot(Archive.main.PullItemFromPool());
        }
    }

    public override void Interact()
    {
        if (!focused)
        {
            FocusIntoShop();
        }
        else
        {
            UnfocusShop();
        }
    }

    void FocusIntoShop()
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.Shop);
        Player.local.MovePlayerToPos(shopViewPos, shopViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    void UnfocusShop()
    {
        focused = false;
        SelectActiveShopSlot(null);
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape) && focused)
        {
            UnfocusShop();
        }
    }

    public void SelectActiveShopSlot(ShopSlot slot)
    {
        if (slot == null)
        {
            activeSlot = null;
            tooltipBox.gameObject.SetActive(false);
        }
        else if (slot != activeSlot)
        {
            activeSlot = slot;
            tooltipBox.gameObject.SetActive(true);
            tooltipBox.position = slot.transform.position;

            int index = 0;
            for (int i = 0; i < shopSlots.Length; i++)
            {
                if (shopSlots[i] == slot)
                {
                    index = i;
                    break;
                }
            }

            Debug.Log(index);

            if (index == 4) { tooltipBox.transform.GetChild(0).localPosition = new Vector3(-0.525f, 0, 0.767f); }
            else { tooltipBox.transform.GetChild(0).localPosition = new Vector3(-0.525f, 0, -0.767f); }

            ShopItem item = slot.ItemInSlot();
            tooltipTitle.text = item.ItemName;
            tooltipDesc.text = item.ItemDescription;
            tooltipType.text = item.Type.ToString();
        }
    }

    public void BuyItemInSlot(int slot)
    {

    }
}

