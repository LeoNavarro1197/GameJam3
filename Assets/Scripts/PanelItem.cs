using UnityEngine;

public class PanelItem : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public AudioSource audioSource;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waveSpawner.isItemView = false;
        Invoke("Paused", .7f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Paused()
    {
        Time.timeScale = 0.1f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        audioSource.pitch = 1;
        waveSpawner.isItemView = true;
        gameObject.SetActive(false);
    }
}
