using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TextMeshPro scoreText;
    private int score = 0;
    public AmmoRaycastScript raycastScript;
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause(true);
        }
    }

    public void TogglePause(bool isPaused)
    {
        pausePanel.SetActive(isPaused);
        this.isPaused = isPaused;
        Time.timeScale = isPaused ? 0f : 1f;
    }


    public bool IsPaused()
    {
        return isPaused;
    }

    public void OnResumeClicked()
    {
        TogglePause(false);
        raycastScript.EnableInputAfterDelay(0.1f); // espera 100 ms para ignorar el clic
    }

    public void OnRestartClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnBackToMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Title-Menu");
    }

    // Llamado cuando el jugador es golpeado por un enemigo
    public void TriggerGameOver()
    {
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f;

        // Mostrar puntuación acumulada (simulada o recibida de otro script)
        scoreText.text = "Puntuación: " + score.ToString();
    }

    // Método público que otros scripts pueden usar para sumar puntos
    public void AddScore(int amount)
    {
        score += amount;
    }
}

