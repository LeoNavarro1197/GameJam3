using System.Collections;
using UnityEngine;

public class AmmoRaycastGun2 : MonoBehaviour
{
    public RevolverScriptUIGun2 ammoUI; // Referencia al script que controla la animación
    public GameManagerScriptGun2 gameManagerScript; // Referencia al controlador de estados
    public PlayerController playerController;
    public WaveSpawner waveSpawner;

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
        if (waveSpawner.isItemView)
        {
            if (playerController.isDead)
            {
                if (inputEnabled && Input.GetMouseButtonDown(1) && !gameManagerScript.IsPaused())
                {
                    ammoUI.OnClickAdvanceAmmo();
                }
            }
        }
    }
}