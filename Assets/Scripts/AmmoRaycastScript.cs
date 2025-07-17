using System.Collections;
using UnityEngine;

public class AmmoRaycastScript : MonoBehaviour
{
    public RevolverScriptUI ammoUI; // Referencia al script que controla la animación
    public GameManagerScript gameManagerScript; // Referencia al controlador de estados
    public PlayerController playerController;

    private bool inputEnabled = true;

    private void Start()
    {
        playerController = GameObject.FindAnyObjectByType<PlayerController>();
    }

    public void EnableInputAfterDelay(float delay)
    {
        inputEnabled = false;
        StartCoroutine(DelayedEnableInput(delay));
    }

    IEnumerator DelayedEnableInput(float delay)
    {
        yield return new WaitForSecondsRealtime(delay); // Usa "Realtime" para que funcione con Time.timeScale = 0
        inputEnabled = true;
    }

    void Update()
    {
        if (inputEnabled && Input.GetMouseButtonDown(0) && !gameManagerScript.IsPaused())
        {
            if (playerController.isDead)
            {
                ammoUI.OnClickAdvanceAmmo();
            }
            
        }
    }
}