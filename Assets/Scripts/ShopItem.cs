using UnityEngine;

public class ShopItem : MonoBehaviour
{
    public string ItemName {  get; private set; }
    [TextArea(3,10)]
    public string ItemDescription { get; private set; }
    public int ItemCost { get; private set; }
}
