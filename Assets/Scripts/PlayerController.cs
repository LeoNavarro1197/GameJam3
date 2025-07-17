using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform firePoint;
    public RevolverScriptUI revolverScriptUI;
    GameManagerScript gameManagerScript;

    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;
    public float rollForwardSpeed = 5f;
    public float rollForwardDuration = 1f;
    public float shootAnimationDuration = 1f;

    private float rollForwardTimer = 0f;
    private float shootTimer = 0f;
    private bool isShooting = false;

    public bool isDead = true;

    private Vector3 forward, right;

    public Camera mainCamera;
    public Animator animator;

    void Start()
    {
        revolverScriptUI = GameObject.FindAnyObjectByType<RevolverScriptUI>();
        gameManagerScript = GameObject.FindAnyObjectByType<GameManagerScript>();

        forward = mainCamera.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);

        right = mainCamera.transform.right;
        right.y = 0;
        right = Vector3.Normalize(right);

        Camera.main.backgroundColor = Color.black;
    }

    void Update()
    {
        animator.SetBool("is_shot", true);

        if (isDead)
        {
            // === Rodar hacia adelante ===
            /*if (rollForwardTimer > 0)
            {
                rollForwardTimer -= Time.deltaTime;
                transform.position += transform.forward * rollForwardSpeed * Time.deltaTime;

                animator.SetBool("is_rollforward", true);
                ResetMovementAnimations();
                return;
            }
            else
            {
                animator.SetBool("is_rollforward", false);
            }*/

            // === Movimiento ===
            float horizontalInput = Input.GetAxis("Horizontal");
            float verticalInput = Input.GetAxis("Vertical");

            Vector3 direction = horizontalInput * right + verticalInput * forward;

            if (direction.magnitude > 0.1f)
            {
                transform.position += direction * moveSpeed * Time.deltaTime;

                float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

                ResetMovementAnimations();

                if (angle >= -45f && angle < 45f)
                    animator.SetBool("is_runing", true);             // Adelante
                else if (angle >= 45f && angle < 135f)
                    animator.SetBool("is_straferight", true);        // Derecha
                else if (angle <= -45f && angle > -135f)
                    animator.SetBool("is_strafeleft", true);         // Izquierda
                else
                    animator.SetBool("is_runingback", true);         // Atrás
            }
            else
            {
                ResetMovementAnimations();
            }

            // === Rodar con espacio ===
            /*if (Input.GetKeyDown(KeyCode.Space))
            {
                rollForwardTimer = rollForwardDuration;
            }*/

            // === Rotar hacia el mouse ===
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Vector3 lookDirection = hit.point - transform.position;
                lookDirection.y = 0;
                if (lookDirection != Vector3.zero)
                {
                    Quaternion targetRotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(lookDirection), rotationSpeed * Time.deltaTime);
                    transform.rotation = targetRotation;
                }
            }

            // === Disparo con clic izquierdo ===
            if (Input.GetMouseButtonDown(0) && !isShooting)
            {
                if (revolverScriptUI.frameIndex < 13)
                {
                    StartCoroutine(ShootAfterDelay());
                }

            }

            if (shootTimer > 0)
            {
                shootTimer -= Time.deltaTime;
            }
            else
            {
                //animator.SetBool("is_shot", false);
            }
        }

        
    }

    private System.Collections.IEnumerator ShootAfterDelay()
    {
        isShooting = true;
        animator.SetBool("is_shot", true);
        shootTimer = shootAnimationDuration;

        yield return new WaitForSeconds(shootAnimationDuration);

        // Plano horizontal a la altura del firePoint
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, firePoint.position.y, 0));
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        Vector3 targetDirection = transform.forward;

        if (groundPlane.Raycast(ray, out float enter))
        {
            Vector3 hitPoint = ray.GetPoint(enter);
            targetDirection = hitPoint - firePoint.position;
            targetDirection.y = 0;
            targetDirection.Normalize();
        }

        // Rotación para que el proyectil apunte al target horizontalmente
        Quaternion projectileRotation = Quaternion.LookRotation(targetDirection);

        // Instanciamos el proyectil con rotación calculada
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, projectileRotation);

        // Inicializamos la dirección en el script Projectile (que debe tener el método Initialize)
        Projectile projScript = projectile.GetComponent<Projectile>();
        if (projScript != null)
        {
            projScript.Initialize(targetDirection);
        }

        // Aplicar rotación local para corregir modelo visual (hijo llamado "Visual")
        Transform visual = projectile.transform.Find("Visual");
        if (visual != null)
        {
            visual.localRotation = Quaternion.Euler(-90f, 0f, 0f);
        }

        isShooting = false;
    }

    public void Die()
    {
        animator.SetBool("is_dead", true);
        isDead = false;
        Invoke("GameOver", 2);
    }

    void GameOver()
    {
        gameManagerScript.TriggerGameOver();
    }

    private void ResetMovementAnimations()
    {
        animator.SetBool("is_runing", false);
        animator.SetBool("is_runingback", false);
        animator.SetBool("is_strafeleft", false);
        animator.SetBool("is_straferight", false);
    }
}

