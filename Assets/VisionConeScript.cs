using UnityEngine;

public class VisionCone : MonoBehaviour
{
    [SerializeField] private bool familySide = true;

    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerMovement2D playerState = other.GetComponent<PlayerMovement2D>();
        if (playerState != null && playerState.masc && !familySide)
        {
            FindAnyObjectByType<GameManagerScript>().GameOver();
        }
        if (playerState != null && !playerState.masc && familySide)
        {
            FindAnyObjectByType<GameManagerScript>().GameOver();
        }
        if (playerState != null && !playerState.masc && !familySide)
        {
            print("Lenora beloved!");
            FindAnyObjectByType<GameManagerScript>().IncreaseFriendScore();
        }
        if (playerState != null && playerState.masc && familySide)
        {
            print("Lenardo beloved!");
            FindAnyObjectByType<GameManagerScript>().IncreaseFamilyScore();
        }
    }
}