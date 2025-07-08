using TMPro;
using UnityEngine;

public class TimerScript : MonoBehaviour
{
    public float timeRemaining = 180f; // 3 minutes in seconds
    private bool timerIsRunning = true;
    private TextMeshProUGUI timerText;
    [SerializeField] private GameObject winScreen;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        UpdateTimerDisplay();
    }

    private void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerDisplay();
            }
            else
            {
                timeRemaining = 0;
                timerIsRunning = false;
                UpdateTimerDisplay();
                OnTimerEnd();
            }
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(timeRemaining / 60);
        int seconds = Mathf.FloorToInt(timeRemaining % 60);
        timerText.text = $"Rumpus ends in: {minutes:00}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        winScreen.SetActive(true);
        Time.timeScale = 0f;
        // Optional: trigger an event, end game, etc.
    }
}