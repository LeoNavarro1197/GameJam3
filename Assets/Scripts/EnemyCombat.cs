using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public float killRange = 2f;
    public float detectionRange = 1f;
    public float attackAnimationDuration = .5f; // Ajusta según tu animación

    private Transform player;
    private Animator animator;
    private NavMeshAgent agent;
    private bool isAttacking = false;
    private bool playerKilled = false;

    public bool isDeadPlayer = true;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (playerKilled) return;

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= killRange && !isAttacking)
        {
            StartAttack();
            agent.isStopped = true;
        }
        else if (distanceToPlayer <= detectionRange && !isAttacking)
        {
            agent.isStopped = false;
            agent.SetDestination(player.position);
        }
    }

    public void StartAttack()
    {
        // Cambia "isAttacking" por el nombre de tu parámetro bool
        animator.SetBool("isAttack", true);
        isAttacking = true;
        playerKilled = false;

        StartCoroutine(WaitForAttackToFinish());
    }

    IEnumerator WaitForAttackToFinish()
    {
        // Espera el tiempo de la animación
        yield return new WaitForSeconds(attackAnimationDuration);

        // Verifica si el player sigue cerca
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= killRange)
        {
            KillPlayer();
        }

        // Desactiva el parámetro de ataque
        animator.SetBool("isAttack", false);

        playerKilled = true;
        isAttacking = false;
        agent.isStopped = false;
    }

    public void KillPlayer()
    {
        PlayerController playerController = player.GetComponent<PlayerController>();
        if (playerController != null)
        {
            isDeadPlayer = false;
            playerController.Die();
        }
    }
}