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

    bool paused;

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
        AudioManager.main.SwitchToGameMusic();
        RunLogger.main.StartNewRun();
        canaryStory.OnStartRun();
    }

    public void StartNextRun()
    {
        ResetGame();
        StartRun();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        paused = true;
        UIController.main.Paused();
    }

    public bool IsPaused() { return paused; }

    public void UnpauseGame()
    {
        Time.timeScale = 1f;
        paused = false;
        UIController.main.Unpaused();
    }

    public void LastSpinUsed()
    {
        countDown = countDownStart;
        UIController.main.ToggleCountDownUI(true);
        UIController.main.CountDownUI((Mathf.Round(countDown * 100)/100).ToString());
        AudioManager.main.PlayCountdownMusic();
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
        AudioManager.main.SwitchToGameOverMusic();
    }

    public void FiredUpFurnance()
    {
        if (countingDown)
        {
            UIController.main.ToggleCountDownUI(false);
            countingDown = false;
            trainCabinAnimator.StopCrushAnimation();
            AudioManager.main.StopCountdownMusic();
        }
    }
}
