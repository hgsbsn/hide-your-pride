using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject creditsPanel;
    [SerializeField] private AudioSource rawr;

    public void LoadLevel()
    {
        DontDestroyOnLoad(rawr);
        rawr.Play();
        SceneManager.LoadScene("Tutorial");
    }

    public void ShowCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void HideCredits()
    {
        creditsPanel.SetActive(false);
    }
}