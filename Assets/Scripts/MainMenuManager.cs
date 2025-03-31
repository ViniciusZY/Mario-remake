using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenOptions()
    {
        Debug.Log("Options menu ainda não implementado");
    }

    public void ExitGame()
    {
        Application.Quit();
        Debug.Log("Jogo fechado");
    }
}
