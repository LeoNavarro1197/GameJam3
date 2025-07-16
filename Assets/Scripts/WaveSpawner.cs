using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;  // Para el texto de feedback

public class WaveSpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;  // Tus spawns definidos en escena
    public int startingEnemies = 5;   // Enemigos en oleada 1
    public int enemiesIncreasePerWave = 2;  // Incremento enemigos por oleada
    public int maxEnemiesOnScreen = 20;
    public float timeBetweenWaves = 5;
    public TextMeshProUGUI waveMessageText;  // UI Text para mostrar "Oleada X"

    private int currentWave = 0;
    private int enemiesToSpawn = 0;
    private int enemiesAlive = 0;
    private bool spawning = false;
    private Coroutine messageCoroutine; // Referencia específica para el mensaje

    void Start()
    {
        StartCoroutine(ManageWaves());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log($"[TEST] enemiesAlive = {enemiesAlive}, spawning = {spawning}");
        }
    }

    IEnumerator ManageWaves()
    {
        while (true)
        {
            // 1. Subir oleada
            currentWave++;
            enemiesToSpawn = Mathf.Min(startingEnemies + enemiesIncreasePerWave * (currentWave - 1), maxEnemiesOnScreen);
            Debug.Log($"Oleada {currentWave} - Enemigos a generar: {enemiesToSpawn}");

            // 2. Mostrar mensaje (opcional)
            ShowWaveMessage($"Oleada {currentWave}");

            // 3. Espera antes de iniciar
            yield return new WaitForSecondsRealtime(timeBetweenWaves);

            // 4. Iniciar spawn
            spawning = true;
            StartCoroutine(SpawnEnemies());

            // 5. Esperar hasta que TODOS estén muertos y ya no esté spawneando
            while (enemiesAlive > 0 || spawning)
            {
                Debug.Log($"Esperando... Enemigos vivos: {enemiesAlive}, spawning: {spawning}");
                yield return null;
            }

            Debug.Log("Oleada terminada");
        }
    }

    IEnumerator SpawnEnemies()
    {
        int spawnedCount = 0;
        while (spawnedCount < enemiesToSpawn)
        {
            // No exceder máximo enemigos activos
            if (enemiesAlive < maxEnemiesOnScreen)
            {
                Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
                //Debug.Log($"Spawneando enemigo en {spawnPoint.position}");
                enemiesAlive++;
                spawnedCount++;
            }
            yield return new WaitForSeconds(1f); // intervalo entre spawns, puedes ajustar
        }
        Debug.Log("Terminó SpawnEnemies()");
        spawning = false;
    }

    public void OnEnemyKilled()
    {
        enemiesAlive = Mathf.Max(0, enemiesAlive - 1);
        Debug.Log("enemigos vivos: " + enemiesAlive);
    }

    void ShowWaveMessage(string message)
    {
        if (waveMessageText != null)
        {
            // Detener solo la corrutina del mensaje anterior si existe
            if (messageCoroutine != null)
            {
                StopCoroutine(messageCoroutine);
            }
            messageCoroutine = StartCoroutine(ShowMessageCoroutine(message));
        }
    }

    IEnumerator ShowMessageCoroutine(string message)
    {
        waveMessageText.text = message;
        waveMessageText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3f);
        waveMessageText.gameObject.SetActive(false);
        messageCoroutine = null; // Limpiar la referencia
    }
}