using UnityEngine;

public class PostBox : Interactable
{
    [SerializeField] PostOption[] boxOptions;
    [SerializeField] string[] postTitles;
    [SerializeField] string[] postMessages;

    [SerializeField] Transform lightHolder;
    [SerializeField] float lightRotateSpeed;

    [SerializeField] BoxCollider boxCollider;
    [SerializeField] Transform cameraFocalPoint;
    [SerializeField] Vector3 boxViewPos, boxViewRot;
    bool focused = false;


    public override void Interact()
    {
        if (HasPost())
        {
            FocusOnPost();
        }
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && focused)
        {
            ExitPost();
        }

        if (HasPost())
        {
            lightHolder.localRotation = Quaternion.Euler(lightHolder.localEulerAngles + (Vector3.up * lightRotateSpeed * Time.deltaTime));
        }
    }

    public bool HasPost()
    {
        return boxOptions[0] != null;
    }

    public void FocusOnPost()
    {
        focused = true;
        Player.local.TakeControlOfCamera(StarterAssets.FirstPersonController.Controller.PostBox);
        Player.local.MovePlayerToPos(boxViewPos, boxViewRot);
        Player.local.ForceLookAt(cameraFocalPoint);
        boxCollider.enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        ShowPostUI();
    }

    void ShowPostUI()
    {
        int i = Random.Range(0, postTitles.Length);

        UIController.main.ShowPostScreen(postTitles[i], postMessages[i], boxOptions);
    }

    public void ExitPost()
    {
        focused = false;
        EmptyPost();
        Player.local.ReleaseControlOfCamera();
        boxCollider.enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void PopulatePost()
    {
        for (int i = 0; i < boxOptions.Length; i++)
        {
            boxOptions[i] = Archive.main.PullPostFromPool();
        }

        lightHolder.gameObject.SetActive(true);
    }

    public void EmptyPost()
    {
        for(int i = 0;i < boxOptions.Length; i++)
        {
            if(boxOptions[i] != null)
            {
                Archive.main.AddToPostPool(boxOptions[i]);
                boxOptions[i] = null;
            }
        }

        lightHolder.gameObject.SetActive(false);
    }

    public void ChoosePostReward(int slot)
    {
        PostOption post = boxOptions[slot];

        switch (post.Reward)
        {
            case PostOption.PostReward.GainResource:
                switch (post.ResourceType)
                {
                    case RewardProfile.RewardType.Coins: CoinSpawner.main.SpawnCoins(post.Strength); break;
                    case RewardProfile.RewardType.Fuel: RewardShoot.main.SpawnFuelReward(post.Strength); break;
                }
                break;
            case PostOption.PostReward.AdjustColourRewards:
                foreach (WheelSegment.SegmentColour c in post.PositiveSegmentColour) 
                    { Archive.main.RewardProfileForSegmentColour(c).IncreaseAllRewards(post.Strength); }
                foreach (WheelSegment.SegmentColour c in post.NegativeSegmentColour)
                    { Archive.main.RewardProfileForSegmentColour(c).DecreaseAllRewards(post.Strength); }
                break;
            case PostOption.PostReward.AdjustColourSize:
                foreach (WheelSegment.SegmentColour c in post.PositiveSegmentColour) { Wheel.main.AddToSegment(post.Strength, c); }
                foreach (WheelSegment.SegmentColour c in post.NegativeSegmentColour) { Wheel.main.AddToSegment(-post.Strength, c); }
                break;
            case PostOption.PostReward.SootTradeOff:
                break;
            case PostOption.PostReward.AdjustLargestColourSize: 
                foreach (WheelSegment.SegmentColour c in Wheel.main.LargestSegment()) { Wheel.main.AddToSegment(post.Strength, c); }
                break;
            case PostOption.PostReward.AdjustSmallestColourSize:
                foreach (WheelSegment.SegmentColour c in Wheel.main.SmallestSegment()) { Wheel.main.AddToSegment(post.Strength, c); }
                break;
        }
    }
}
