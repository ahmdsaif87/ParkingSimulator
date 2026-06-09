using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public enum GameState { Level1, Level2, GameOver }

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameState currentState = GameState.Level1;
    public TrafficCar[] trafficCars;
    public CarController playerCarController;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    Rigidbody playerRb;
    bool hasShownStart;
    bool hasStartedMoving;

    void Start()
    {
        SetState(GameState.Level1);
        if (playerCarController != null)
            playerRb = playerCarController.GetComponent<Rigidbody>();
        hasShownStart = false;
        hasStartedMoving = false;
    }

    void Update()
    {
        if (currentState != GameState.Level1 || hasShownStart) return;
        if (playerRb == null) return;

        if (!hasStartedMoving && playerRb.linearVelocity.sqrMagnitude > 0.1f)
        {
            hasStartedMoving = true;
            if (UIManager.Instance != null)
                UIManager.Instance.ShowStartText();
            StartCoroutine(HideStartAfterDelay(2f));
        }
    }

    IEnumerator HideStartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        hasShownStart = true;
        if (UIManager.Instance != null)
            UIManager.Instance.HideStartText();
    }

    public void SetState(GameState newState)
    {
        currentState = newState;
        switch (newState)
        {
            case GameState.Level1:
                EnableTraffic(false);
                ResetPlayer();
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("LEVEL 1 - Park Your Car!", 2f);
                break;

            case GameState.Level2:
                EnableTraffic(true);
                ResetPlayer();
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("LEVEL 2 - Traffic Cars Moving!", 2f);
                break;

            case GameState.GameOver:
                EnableTraffic(false);
                if (UIManager.Instance != null)
                    UIManager.Instance.ShowMessage("GAME OVER!", 0f);
                break;
        }
    }

    public void OnParkingSuccess()
    {
        if (currentState == GameState.Level1)
        {
            if (playerCarController != null)
                playerCarController.enabled = false;
            if (playerRb != null)
            {
                playerRb.linearVelocity = Vector3.zero;
                playerRb.angularVelocity = Vector3.zero;
            }
            if (UIManager.Instance != null)
                UIManager.Instance.ShowSuccessPanel("Parkir Berhasil", "Level 1 Selesai");
        }
        else if (currentState == GameState.Level2)
        {
            if (UIManager.Instance != null)
                UIManager.Instance.ShowMessage("YOU WIN!", 2f);
            StartCoroutine(DelayedAction(3f, () => SceneManager.LoadScene("MainMenu")));
        }
    }

    public void OnGameOver()
    {
        SetState(GameState.GameOver);
        StartCoroutine(DelayedAction(3f, () => SceneManager.LoadScene("MainMenu")));
    }

    void EnableTraffic(bool enable)
    {
        if (trafficCars == null) return;
        foreach (var t in trafficCars)
        {
            if (t != null) t.enabled = enable;
        }
    }

    void ResetPlayer()
    {
        var car = GameObject.Find("MuscleCar");
        if (car == null) return;

        car.transform.position = new Vector3(120f, 0.35f, 80f);
        car.transform.rotation = Quaternion.Euler(0f, 180f, 0f);
        var rb = car.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        var health = car.GetComponent<CarHealth>();
        if (health != null) health.ResetHealth();

        var cc = car.GetComponent<CarController>();
        if (cc != null) cc.enabled = true;
    }

    IEnumerator DelayedAction(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        if (action != null) action();
    }
}
