using System.Collections;
using UnityEngine;

public class AmmoRaycastScript : MonoBehaviour
{
    public RevolverScriptUI ammoUI; // Referencia al script que controla la animación
    public GameManagerScript gameManagerScript; // Referencia al controlador de estados

    private bool inputEnabled = true;

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
            ammoUI.OnClickAdvanceAmmo();
        }
    }
}