using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    public float familyScore = 100f;
    public float friendScore = 100f;
    [SerializeField] private float scoreDrain = 15f;
    [SerializeField] private float initialWait = 5f;
    [SerializeField] private float drainInterval = 5f;
    public float familyTimeTimer = 20f;
    public float friendTimeTimer = 20f;
    public float timeSpentTimerReset = 20f;
    public float familyTimeDrainMult = 1f;
    public float friendTimeDrainMult = 1f;
    public float goodTimerStanding = 10f;
    public float midTimerStanding = 5f;
    public float badTimerStanding = 0f;
    [SerializeField] private HeartFill familyHeartFill;
    [SerializeField] private HeartFill friendHeartFill;

    private void Start()
    {
        StartCoroutine(DrainScoresOverTime());
    }

    private void Update()
    {
        familyHeartFill.UpdateFamilyHeartFill(familyScore);
        friendHeartFill.UpdateFriendHeartFill(friendScore);
        if (familyScore <= 0 || friendScore <= 0)
        {
            GameOver();
        } 
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        Time.timeScale = 0f;
        Debug.Log("Game Over!");
    }

    public void ResetScene()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Finale()
    {
        SceneManager.LoadScene("Finale");
    }

    public void IncreaseFamilyScore()
    {
        familyScore++;
        //print("Family Score: " + familyScore);
    }

    public void IncreaseFriendScore()
    {
        friendScore++;
        //print("Friend Score: " + friendScore);
    }

    private IEnumerator DrainScoresOverTime()
    {
        yield return new WaitForSeconds(initialWait); // Initial wait

        while (true)
        {
            if(familyTimeTimer >= goodTimerStanding)
            {
                familyTimeDrainMult = 0f;
            }
            else if(familyTimeTimer >= midTimerStanding)
            {
                familyTimeDrainMult = .5f;
            }
            else if (familyTimeTimer >= badTimerStanding)
            {
                familyTimeDrainMult = 1f;
            }
            if (friendTimeTimer >= goodTimerStanding)
            {
                friendTimeDrainMult = 0f;
            }
            else if (friendTimeTimer >= midTimerStanding)
            {
                friendTimeDrainMult = .5f;
            }
            else if (friendTimeTimer >= badTimerStanding)
            {
                friendTimeDrainMult = 1f;
            }


            familyScore -= scoreDrain * familyTimeDrainMult;
            friendScore -= scoreDrain * friendTimeDrainMult;

            // Clamp scores to 0 to avoid going negative (optional)
            familyScore = Mathf.Max(familyScore, 0f);
            friendScore = Mathf.Max(friendScore, 0f);

            //Debug.Log($"Family: {familyScore} | Friend: {friendScore}");

            yield return new WaitForSeconds(drainInterval);
        }
    }
}