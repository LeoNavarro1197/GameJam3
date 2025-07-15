using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject powerUpPrefab;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnInterval = 20f;

    [Header("Spawn Area (XZ limits)")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;
    [SerializeField] private float minZ = -10f;
    [SerializeField] private float maxZ = 10f;

    private void Start()
    {
        InvokeRepeating(nameof(SpawnPowerUp), spawnInterval, spawnInterval);
    }

    private void SpawnPowerUp()
    {
        Vector3 spawnPosition = new Vector3(
            Random.Range(minX, maxX),
            0.5f,
            Random.Range(minZ, maxZ)
        );

        GameObject spawned = Instantiate(powerUpPrefab, spawnPosition, Quaternion.identity);

        spawned.transform.position = new Vector3(spawnPosition.x, 0.5f, spawnPosition.z);
    }
}
