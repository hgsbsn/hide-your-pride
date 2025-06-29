using TMPro;
using UnityEngine;

public class TimeSpentScript : MonoBehaviour
{
    [SerializeField] private GameManagerScript gameManager;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private bool family = true;

    private void Start()
    {
        timerText = GetComponent<TextMeshProUGUI>();
        UpdateFriendTimerDisplay();
        UpdateFamilyTimerDisplay();

    }

    private void Update()
    {
        if (family)
        {
            if (gameManager.familyTimeTimer > 0)
            {
                gameManager.familyTimeTimer -= Time.deltaTime;
                UpdateFamilyTimerDisplay();
            }
            else
            {
                gameManager.familyTimeTimer = 0;
                UpdateFamilyTimerDisplay();
                OnTimerEnd();
            }
        }
        else
        {
            if (gameManager.friendTimeTimer > 0)
            {
                gameManager.friendTimeTimer -= Time.deltaTime;
                UpdateFriendTimerDisplay();
            }
            else
            {
                gameManager.friendTimeTimer = 0;
                UpdateFriendTimerDisplay();
                OnTimerEnd();
            }

        }
    }

    private void UpdateFriendTimerDisplay()
    {
        print("updating timer");
        int minutes = Mathf.FloorToInt(gameManager.friendTimeTimer / 60);
        int seconds = Mathf.FloorToInt(gameManager.friendTimeTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void UpdateFamilyTimerDisplay()
    {
        print("updating timer");
        int minutes = Mathf.FloorToInt(gameManager.familyTimeTimer / 60);
        int seconds = Mathf.FloorToInt(gameManager.familyTimeTimer % 60);
        timerText.text = $"{minutes:00}:{seconds:00}";
    }

    private void OnTimerEnd()
    {
        Debug.Log("Timer finished!");
        // Optional: trigger an event, end game, etc.
    }
}