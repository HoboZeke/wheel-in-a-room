using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Archive : MonoBehaviour
{
    public static Archive main;

    [SerializeField] ShopItem[] shopItems;
    List<ShopItem> shopItemPool = new List<ShopItem>();

    [SerializeField] WheelSegment.SegmentColour colourLookup;
    [SerializeField] RewardProfile[] colourProfiles;
    [SerializeField] Color[] colourProfileColours;

    [SerializeField] PostOption[] postOptions;
    List<PostOption> postOptionsPool = new List<PostOption>();

    private void Awake()
    {
        main = this;
        shopItemPool.AddRange(shopItems);
        postOptionsPool.AddRange(postOptions);
    }

    private void Start()
    {
        foreach (RewardProfile p in colourProfiles) { p.Setup(); }
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

    public RewardProfile RewardProfileForSegmentColour(WheelSegment.SegmentColour colour)
    {
        return colourProfiles[(int)colour];
    }

    public Color ColourForColourProfile(WheelSegment.SegmentColour colour)
    {
        return colourProfileColours[(int)colour];
    }

    public PostOption PullPostFromPool()
    {
        if (postOptionsPool.Count > 0)
        {
            int i = Random.Range(0, postOptionsPool.Count);
            PostOption po = postOptionsPool[i];
            postOptionsPool.RemoveAt(i);
            return po;
        }
        else
        {
            return postOptions[0];
        }
    }

    public void AddToPostPool(PostOption post)
    {
        postOptionsPool.Add(post);
    }

}
