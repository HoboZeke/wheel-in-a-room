using UnityEngine;

[CreateAssetMenu(fileName = "ShopItem", menuName = "ScriptableObjects/ShopItem")]
public class ShopItem : ScriptableObject
{
    //Inspector Values
    [SerializeField] string itemName;
    [TextArea(3,10)]
    [SerializeField] string itemDescription;
    [SerializeField] int itemCost;
    [SerializeField] Mesh mesh;
    [SerializeField] Material material;
    [SerializeField] ItemType type;
    [SerializeField] ItemRarity rarity;

    public enum ItemType { Wedge, Arrow, Trinket }
    public enum ItemRarity { Common, Uncommon, Rare, Unique }

    public string ItemName {  get { return itemName; } private set { itemName = value; } }
    public string ItemDescription { get { return itemDescription; } private set { itemDescription = value; } }
    public int ItemCost { get { return itemCost; } private set { itemCost = value; } }
    public Mesh ItemMesh { get { return mesh; } private set { mesh = value; } }
    public Material ItemMaterial { get { return material; } private set { material = value; } }
    public ItemType Type { get { return type; } private set { type = value; } }
    public ItemRarity Rarity { get { return rarity; } private set { rarity = value; } }
}
