using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rollForwardSpeed = 5f;
    public float rollForwardDuration = 1f;
    public float shootAnimationDuration = 1f;  

    private float rollForwardTimer = 0f;
    private float shootTimer = 0f;
    private bool isShooting = false;

    private Vector3 forward, right;

    public Camera mainCamera;
    public Animator animator;

    void Start()
    {
        forward = mainCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = mainCamera.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);
    }

    void Update()
    {
        if (rollForwardTimer > 0)
        {
            rollForwardTimer -= Time.deltaTime;
            transform.position += transform.forward * rollForwardSpeed * Time.deltaTime;

            animator.SetBool("is_rollforward", true);
            animator.SetBool("is_runing", false);
            return;
        }
        else
        {
            animator.SetBool("is_rollforward", false);
        }

        // Movimiento normal
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = horizontalInput * right + verticalInput * forward;

        if (direction.magnitude > 0.1f)
        {
            transform.position += direction * moveSpeed * Time.deltaTime;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            animator.SetBool("is_runing", true);
        }
        else
        {
            animator.SetBool("is_runing", false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rollForwardTimer = rollForwardDuration;
        }

        if (Input.GetKeyDown(KeyCode.Return) && !isShooting)
            {
                StartCoroutine(ShootAfterDelay());
            }


        if (shootTimer > 0)
        {
            shootTimer -= Time.deltaTime;
        }
        else
        {
            animator.SetBool("is_shot", false);
        }
    }

    private System.Collections.IEnumerator ShootAfterDelay()
    {
        isShooting = true;
        animator.SetBool("is_shot", true);
        shootTimer = shootAnimationDuration;

        yield return new WaitForSeconds(shootAnimationDuration);

        Instantiate(projectilePrefab, firePoint.position, firePoint.rotation);

        isShooting = false;
    }
}
