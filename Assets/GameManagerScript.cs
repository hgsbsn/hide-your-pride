using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    public float familyScore = 0f;
    public float friendScore = 0f;
    [SerializeField] private float scoreDrain = 15f;
    [SerializeField] private float initialWait = 5f;
    [SerializeField] private float drainInterval = 5f;

    private void Start()
    {
        StartCoroutine(DrainScoresOverTime());
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
            familyScore -= scoreDrain;
            friendScore -= scoreDrain;

            // Clamp scores to 0 to avoid going negative (optional)
            familyScore = Mathf.Max(familyScore, 0f);
            friendScore = Mathf.Max(friendScore, 0f);

            //Debug.Log($"Family: {familyScore} | Friend: {friendScore}");

            yield return new WaitForSeconds(drainInterval);
        }
    }
}