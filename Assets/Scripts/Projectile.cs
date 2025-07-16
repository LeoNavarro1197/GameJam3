using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 20f;
    public float maxDistance = 50f;

    private Vector3 startPosition;
    private Vector3 moveDirection;

    // Nueva función para inicializar el proyectil
    public void Initialize(Vector3 direction)
    {
        moveDirection = direction.normalized;
    }

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        transform.position += moveDirection * speed * Time.deltaTime;

        if (Vector3.Distance(startPosition, transform.position) > maxDistance)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}

