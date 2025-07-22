using System.Collections.Generic;
using UnityEngine;

public class PowerUpRandom : MonoBehaviour
{
    public List<GameObject> powerUps;
    public List<Transform> pointsPowerUps;

    void Start()
    {
        for (int i = 0; i < powerUps.Count; i++)
        {
            int powerUpCount = Random.Range(0, powerUps.Count);
            powerUps[i].transform.position = pointsPowerUps[powerUpCount].position;
            pointsPowerUps.RemoveAt(powerUpCount);
        }
    }
}
