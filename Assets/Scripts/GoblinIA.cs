using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinIA : MonoBehaviour
{
    public Transform[] puntosPatrulla;
    public float distanciaCorrer = 10f;
    public float distanciaAtaque = 1f;
    public float radioLlegada = 0.5f;
    public bool esperando;

    private NavMeshAgent agente;
    private Animator animator;
    private int indiceDestino = 0;
    private bool persiguiendoJugador = false;
    private Transform jugador;
    private Coroutine esperaCoroutine;
    private Transform objetivoActual;
    private bool atacando = false;
    NavMeshAgent navMeshAgent;

    bool isDead = true;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null) jugador = jugadorGO.transform;
        else Debug.LogError("No se encontró ningún objeto con el tag 'Player'.");

        if (puntosPatrulla.Length == 0)
        {
            Debug.LogWarning("No hay puntos de patrulla asignados.");
        }
        else
        {
            ComenzarPatrullaje();
        }
    }

    void Update()
    {
        if (isDead)
        {
            if (jugador != null)
            {
                float distanciaJugador = Vector3.Distance(transform.position, jugador.position);

                if (distanciaJugador <= distanciaCorrer)
                {
                    if (!persiguiendoJugador)
                    {
                        if (esperaCoroutine != null)
                        {
                            StopCoroutine(esperaCoroutine);
                            esperaCoroutine = null;
                        }
                        agente.isStopped = false;
                        persiguiendoJugador = true;
                    }

                    PerseguirJugador(distanciaJugador);
                }
                else
                {
                    if (persiguiendoJugador)
                    {
                        persiguiendoJugador = false;
                        agente.isStopped = false;
                        objetivoActual = puntosPatrulla[indiceDestino];
                        ComenzarPatrullaje();
                    }

                    Patrullar();
                }
            }
            else
            {
                Patrullar();
            }

            ActualizarAnimaciones();
            RotarHaciaObjetivo();
        }
        
    }

    void PerseguirJugador(float distancia)
    {
        agente.destination = jugador.position;
        objetivoActual = jugador;

        if (distancia > distanciaAtaque)
        {
            agente.speed = 4.5f;
            SetAnimacion(true, false, false); // Correr
            atacando = false;
        }
        else
        {
            agente.speed = 0f;

            if (!atacando)
            {
                StartCoroutine(RealizarAtaque());
            }
        }
    }

    IEnumerator RealizarAtaque()
    {
        atacando = true;
        agente.isStopped = true;
        SetAnimacion(false, true, false); // Atacar

        // Espera el tiempo exacto de la animación de ataque (ajusta si es necesario)
        yield return new WaitForSeconds(1.2f);

        SetAnimacion(false, false, false); // Volver a idle
        agente.isStopped = false;
        atacando = false;
    }

    void Patrullar()
    {
        if (puntosPatrulla.Length == 0 || agente.pathPending) return;

        Vector3 destinoActual = puntosPatrulla[indiceDestino].position;
        float distancia = Vector3.Distance(transform.position, destinoActual);

        if (distancia <= radioLlegada && !esperando)
        {
            if (esperaCoroutine == null)
            {
                esperando = true;
                agente.velocity = Vector3.zero;
                esperaCoroutine = StartCoroutine(EsperarEnPunto(2f));
            }
        }
    }

    IEnumerator EsperarEnPunto(float segundos)
    {
        agente.isStopped = true;
        SetAnimacion(false, false, false); // Idle
        yield return new WaitForSeconds(segundos);
        esperando = false;

        if (!persiguiendoJugador)
        {
            agente.isStopped = false;
            IrAlSiguientePunto();
        }

        esperaCoroutine = null;
    }

    void ComenzarPatrullaje()
    {
        Transform siguiente = puntosPatrulla[indiceDestino];
        agente.destination = siguiente.position;
        objetivoActual = siguiente;
        agente.speed = 2f;
        SetAnimacion(false, false, true); // Caminar
    }

    void IrAlSiguientePunto()
    {
        if (puntosPatrulla.Length == 0) return;
        indiceDestino = (indiceDestino + 1) % puntosPatrulla.Length;
        Transform siguiente = puntosPatrulla[indiceDestino];
        agente.destination = siguiente.position;
        objetivoActual = siguiente;
        agente.speed = 2f;
        SetAnimacion(false, false, true); // Caminar
    }

    void ActualizarAnimaciones()
    {
        if (!animator.GetBool("isRunning") && !animator.GetBool("isAttack"))
        {
            bool estaMoviendose = agente.velocity.magnitude > 0.1f && !agente.isStopped;

            if (esperaCoroutine != null)
                estaMoviendose = false;

            SetAnimacion(false, false, estaMoviendose); // Caminar si se está moviendo
        }
    }

    void SetAnimacion(bool correr, bool atacar, bool caminar)
    {
        animator.SetBool("isRunning", correr);
        animator.SetBool("isAttack", atacar);
        animator.SetBool("isWalking", caminar);
    }

    void RotarHaciaObjetivo()
    {
        if (objetivoActual == null) return;

        Vector3 direccion = objetivoActual.position - transform.position;
        direccion.y = 0;

        if (direccion.magnitude > 0.1f)
        {
            Quaternion rotacion = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacion, Time.deltaTime * 5f);
        }
    }

    void OnDrawGizmos()
    {
        if (puntosPatrulla != null)
        {
            Gizmos.color = Color.green;
            foreach (var punto in puntosPatrulla)
            {
                if (punto != null)
                    Gizmos.DrawSphere(punto.position, 0.3f);
            }
        }
    }

    void Die()
    {
        isDead = false;
        animator.SetBool("isDead", true);
        navMeshAgent.speed = 0;
        Invoke("CallDie", 3);
    }

    void CallDie()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Proyectil")
        {
            Die();
        }
    }
}


