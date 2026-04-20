using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager main;

    [SerializeField] CanaryStory canaryStory;
    [SerializeField] TrainCabinAnimator trainCabinAnimator;
    [SerializeField] float countDownStart;

    [Header("References")]
    [SerializeField] Shop shop;
    [SerializeField] BreakglassCabinet breakglassCabinet;
    [SerializeField] HammerMount hammerMount;
    [SerializeField] Cabinet cabinet;
    float countDown;
    bool countingDown;

    private void Awake()
    {
        main = this;
    }

    public void ResetGame()
    {
        Player.local.ResetToStartPositions();
        Wheel.main.ResetWheel();
        shop.ResetShop();
        Furnance.main.ResetFurnace();
        CoinScoop.main.ResetScoop();
        breakglassCabinet.ResetToStart();
        hammerMount.ResetToStart();
        cabinet.ResetToStart();
        ItemPool.main.ResetInventoryItems();
        PlayerInventory.main.ResetInventory();
    }

    public void StartRun()
    {
        RunLogger.main.StartNewRun();
        canaryStory.OnStartRun();
    }

    public void StartNextRun()
    {
        ResetGame();
        StartRun();
    }

    public void LastSpinUsed()
    {
        countDown = countDownStart;
        UIController.main.ToggleCountDownUI(true);
        UIController.main.CountDownUI((Mathf.Round(countDown * 100)/100).ToString());
        countingDown = true;
    }

    private void Update()
    {
        if (countingDown)
        {
            countDown -= Time.deltaTime;
            if (countDown < 0f) 
            { 
                countDown = 0f;
                GameOver();
                countingDown = false;
                return;
            }

            if (countDown < 3.5f)
            {
                trainCabinAnimator.PlayCrushAnimation();
            }

            if(countDown < 1f)
            {
                UIController.main.SetCountdownBackgroundAlpha((1f - countDown) / 0.75f);
            }

            UIController.main.CountDownUI((Mathf.Round(countDown * 100) / 100).ToString());

        }
    }

    void GameOver()
    {
        UIController.main.GameOverUI();
    }

    public void FiredUpFurnance()
    {
        if (countingDown)
        {
            UIController.main.ToggleCountDownUI(false);
            countingDown = false;
            trainCabinAnimator.StopCrushAnimation();
        }
    }
}
