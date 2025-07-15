using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField] private string powerUpName = "Disparo doble";
    [SerializeField] private float lifetime = 20f;

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
        //TooltipUI.Instance.ShowTemporarily("¡Disparo doble activado!", 3f);
        Destroy(gameObject);
        Debug.Log("Se cogió el power Up");
    }
}


    private void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
