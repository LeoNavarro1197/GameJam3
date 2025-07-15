using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private string powerUpName = "Disparo doble";
    [SerializeField] private float lifetime = 7f;

    private void Start()
    {
        Debug.Log("Power-up apareció");
        Invoke(nameof(SelfDestruct), lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CancelInvoke(nameof(SelfDestruct));
            TooltipUI.Instance.ShowTemporarily(powerUpName, 10f); 
            Destroy(gameObject);
        }
    }

    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
