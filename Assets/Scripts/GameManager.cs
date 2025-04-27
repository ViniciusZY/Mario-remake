using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Lives")]
    private TextMeshProUGUI livesText;
    public int startingLives = 3;
    private int currentLives = 3;

    private TextMeshProUGUI scoreText;
    private int score = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        currentLives = startingLives;
        score = 0;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainMenu")
        {
            Destroy(gameObject);
            return;
        }

        livesText = GameObject.Find("Lives")?.GetComponent<TextMeshProUGUI>();
        scoreText = GameObject.Find("Score")?.GetComponent<TextMeshProUGUI>();
        UpdateLivesUI();
        UpdateScoreUI();
    }

    void UpdateLivesUI()
    {
        if (livesText != null)
            if (currentLives > 1) livesText.text = currentLives + " Lives";
            else livesText.text = currentLives + " Life";
    }

    void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + score;
    }

    public void LoseLife()
    {
        currentLives--;
        UpdateLivesUI();

        if (currentLives > 0)
        {
            StartCoroutine(RestartLevel());
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScoreUI();
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private IEnumerator GameOver()
    {
        yield return new WaitForSeconds(3f);
        AudioManager.instance.PlayMusic(AudioManager.instance.gameOverMusic);
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MainMenu");
    }

    void OnDestroy()
    {
        if (instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
