using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class GoblinIA : MonoBehaviour
{
    public Transform[] puntosPatrulla;
    public float distanciaCorrer = 10f;
    public float distanciaAtaque = 1f;

    private NavMeshAgent agente;
    private Animator animator;
    private int indiceDestino = 0;
    private bool persiguiendoJugador = false;
    private Transform jugador;
    private Coroutine esperaCoroutine;
    private Transform objetivoActual;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null) jugador = jugadorGO.transform;
        else Debug.LogError("No se encontró ningún objeto con el tag 'Player'.");

        if (puntosPatrulla.Length == 0) Debug.LogWarning("No hay puntos de patrulla asignados.");
        else IrAlSiguientePunto();
    }

    void Update()
    {
        if (jugador == null) return;

        float distancia = Vector3.Distance(transform.position, jugador.position);

        if (distancia <= distanciaCorrer)
        {
            if (!persiguiendoJugador)
            {
                if (esperaCoroutine != null)
                {
                    StopCoroutine(esperaCoroutine);
                    esperaCoroutine = null;
                }
                agente.isStopped = false;
            }

            PerseguirJugador(distancia);
        }
        else
        {
            if (persiguiendoJugador)
            {
                persiguiendoJugador = false;
                IrAlSiguientePunto();
            }

            Patrullar();
        }

        ActualizarAnimaciones();
        RotarHaciaObjetivo();
    }

    void PerseguirJugador(float distancia)
    {
        persiguiendoJugador = true;
        agente.destination = jugador.position;
        objetivoActual = jugador;

        if (distancia > distanciaAtaque)
        {
            agente.speed = 4.5f;
            SetAnimacion(true, false, false); // Run
        }
        else
        {
            agente.speed = 2f;
            SetAnimacion(false, true, false); // Attack
        }
    }

    void Patrullar()
    {
        if (!agente.pathPending)
        {
            // Verificación por distancia directa (NO solo remainingDistance)
            float distanciaAlDestino = Vector3.Distance(transform.position, agente.destination);

            if (distanciaAlDestino <= 0.5f) // ← margen más amplio
            {
                if (esperaCoroutine == null)
                {
                    agente.velocity = Vector3.zero; // ← fuerza detención
                    esperaCoroutine = StartCoroutine(EsperarEnPunto(2f));
                }
            }
        }
    }

    IEnumerator EsperarEnPunto(float segundos)
    {
        agente.isStopped = true;
        SetAnimacion(false, false, false); // Idle
        yield return new WaitForSeconds(segundos);

        if (!persiguiendoJugador)
        {
            agente.isStopped = false;
            IrAlSiguientePunto();
        }
        Debug.Log("Esperando en punto...");

        esperaCoroutine = null;
    }

    void IrAlSiguientePunto()
    {
        if (puntosPatrulla.Length == 0) return;

        Transform siguiente = puntosPatrulla[indiceDestino];
        agente.destination = siguiente.position;
        objetivoActual = siguiente;

        Debug.Log("Ir al punto: " + siguiente.name);
        indiceDestino = (indiceDestino + 1) % puntosPatrulla.Length;
        Debug.Log("Cambiando al siguiente punto...");

        agente.isStopped = false;
        agente.speed = 2f;
        SetAnimacion(false, false, true); // walk
    }

    void ActualizarAnimaciones()
    {
        if (!animator.GetBool("isRunning") && !animator.GetBool("isAttack"))
        {
            bool estaMoviendose = agente.velocity.magnitude > 0.1f && !agente.isStopped;

            if (esperaCoroutine != null)
                estaMoviendose = false;

            animator.SetBool("isWalking", estaMoviendose);
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
            Quaternion rotacionDeseada = Quaternion.LookRotation(direccion);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotacionDeseada, Time.deltaTime * 5f);
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
}