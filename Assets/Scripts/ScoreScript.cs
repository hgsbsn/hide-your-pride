using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    private TextMeshProUGUI scoreText;
    private GameManagerScript gameManager;
    [SerializeField] bool familyScore = true;

    private void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
        gameManager = FindAnyObjectByType<GameManagerScript>();
    }

    private void Update()
    {
        if (gameManager != null && familyScore)
        {
            scoreText.text = $"Family Approval: {gameManager.familyScore:F0}";

        }

        if (gameManager != null && !familyScore)
        {
            scoreText.text = $"Friend Approval: {gameManager.friendScore:F0}";
        }
    }
}