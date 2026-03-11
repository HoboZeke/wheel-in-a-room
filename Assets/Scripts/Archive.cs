using System.Collections.Generic;
using UnityEngine;

public class Archive : MonoBehaviour
{
    public static Archive main;

    [SerializeField] ShopItem[] shopItems;
    List<ShopItem> shopItemPool = new List<ShopItem>();

    private void Awake()
    {
        main = this;
        shopItemPool.AddRange(shopItems);
    }

    private void Start()
    {
    }


    public ShopItem PullItemFromPool()
    {
        if (shopItemPool.Count > 0)
        {
            int i = Random.Range(0, shopItemPool.Count);
            ShopItem sI = shopItemPool[i];
            shopItemPool.RemoveAt(i);
            return sI;
        }
        else
        {
            return shopItems[0];
        }
    }

}
