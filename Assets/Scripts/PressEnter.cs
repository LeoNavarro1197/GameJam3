using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PressEnter : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public TextMeshProUGUI textoPulsaEnter;
    public float velocidadParpadeo = 0.5f;
    private float temporizador;

    void Update()
    {
        // Parpadeo
        temporizador += Time.deltaTime;
        if (temporizador >= velocidadParpadeo)
        {
            textoPulsaEnter.enabled = !textoPulsaEnter.enabled;
            temporizador = 0f;
        }

        // Cambio de escena con Enter
        if (Input.GetKeyDown(KeyCode.Return))  // También puedes usar KeyCode.KeypadEnter si prefieres
        {
            SceneManager.LoadScene("Menu-Principal");
        }
    }
    }
