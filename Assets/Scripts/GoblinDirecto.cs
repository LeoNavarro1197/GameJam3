using UnityEngine;
using UnityEngine.AI;

public class GoblinDirecto : MonoBehaviour
{
    public float attackRange = 2f;
    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;
    public WaveSpawner waveSpawner; 
    NavMeshAgent navMeshAgent;
    public GameObject particulaSangre;
    Collider collider;
    AudioSource audioSource;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
        collider = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject != null)
        {
            player = playerObject.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= attackRange)
        {
            // Detenerse y atacar
            animator.SetBool("isRunning", false);
            animator.SetBool("isAttack", true);
            agent.isStopped = true;
            

            // Rotar hacia el jugador suavemente
            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }
        else
        {
            // Perseguir al jugador
            animator.SetBool("isRunning", true);
            animator.SetBool("isAttack", false);
            agent.isStopped = false;
            agent.SetDestination(player.position);
            
        }
    }

    void Die()
    {
        animator.SetBool("isDead", true);
        waveSpawner.OnEnemyKilled();
        navMeshAgent.speed = 0;
        Invoke("CallDie", 6);
    }

    void CallDie()
    {
        Destroy(gameObject);
        Destroy(particulaSangre);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Proyectil")
        {
            particulaSangre.SetActive(true);
            collider.enabled = false;
            audioSource.Play();
            Die();
        }
    }
}


