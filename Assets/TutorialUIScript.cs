using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialUIScript : MonoBehaviour
{

    public GameObject page1;
    public GameObject page2;

    public void NextPage()
    {
        page2.SetActive(true);
        page1.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level");
    }

}
