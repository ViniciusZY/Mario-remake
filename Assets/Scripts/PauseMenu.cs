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
        pausePanel.SetActive(true);
        Time.timeScale = 0; // Congela o jogo
    }

    public void ResumeGame()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1; // Retoma o jogo
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        // Aqui você pode carregar a cena do menu principal. Por enquanto, pode ser uma cena placeholder.
        SceneManager.LoadScene("MainMenu");
    }
}
