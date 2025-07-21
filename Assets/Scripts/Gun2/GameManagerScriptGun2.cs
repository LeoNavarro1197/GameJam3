using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManagerScriptGun2 : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject gameOverPanel;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    public AmmoRaycastGun2 raycastScript;
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

    // Método público que otros scripts pueden usar para sumar puntos
    public void AddScore(int amount)
    {
        score += amount;
        scoreText.text = "Score: " + score.ToString();
    }
}

