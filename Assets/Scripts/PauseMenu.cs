using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Time.timeScale == 1)
                PauseGame();
            else
                ResumeGame();
        }
    }

    public void PauseGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.pauseSound);
        pausePanel.SetActive(true);
        Time.timeScale = 0; 
    }

    public void ResumeGame()
    {
        AudioManager.instance.PlaySFX(AudioManager.instance.pauseSound);
        pausePanel.SetActive(false);
        Time.timeScale = 1; 
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
