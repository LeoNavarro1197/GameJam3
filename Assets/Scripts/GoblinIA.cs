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
    public int indiceDestino = 0;
    private bool persiguiendoJugador = false;
    private Transform jugador;
    private Coroutine esperaCoroutine;
    private Transform objetivoActual;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        // Busca al jugador por tag
        GameObject jugadorGO = GameObject.FindGameObjectWithTag("Player");
        if (jugadorGO != null) jugador = jugadorGO.transform;
        else Debug.LogError("No se encontró ningún objeto con el tag 'Player'.");

        if (puntosPatrulla.Length == 0)
        {
            Debug.LogWarning("No hay puntos de patrulla asignados.");
        }
        else
        {
            ComenzarPatrullaje(); // Comenzar patrullaje
        }

    }

    void Update()
    {
        if (jugador != null)
        {
            float distanciaJugador = Vector3.Distance(transform.position, jugador.position);
            // Cancelar espera si está patrullando
            if (distanciaJugador > distanciaCorrer)
            {
                return;
            }
            if (!persiguiendoJugador && distanciaJugador <= distanciaCorrer)
            {
                if (esperaCoroutine != null)
                {
                    StopCoroutine(esperaCoroutine);
                    esperaCoroutine = null;
                }
                agente.isStopped = false;
            }

            // Iniciar persecución
            persiguiendoJugador = true;
            PerseguirJugador(distanciaJugador);
        }
        else
        {
            // El jugador se alejó
            if (persiguiendoJugador)
            {
                persiguiendoJugador = false;
                agente.isStopped = false;
                IrAlSiguientePunto(); // Reanuda patrulla
            }

            Patrullar(); // Verifica si llegó a un punto
        }

        ActualizarAnimaciones();
        RotarHaciaObjetivo();
    }

    void PerseguirJugador(float distancia)
    {
        agente.destination = jugador.position;
        objetivoActual = jugador;

        if (distancia > distanciaAtaque)
        {
            agente.speed = 4.5f;
            SetAnimacion(true, false, false); // Correr
        }
        else
        {
            agente.speed = 2f;
            SetAnimacion(false, true, false); // Atacar
        }
    }

    [NaughtyAttributes.Button]
 void Pruebame()
    {
        print("Ya me canse");
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
                esperaCoroutine = StartCoroutine(EsperarEnPunto(2f)); // Idle 2 segundos
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
}