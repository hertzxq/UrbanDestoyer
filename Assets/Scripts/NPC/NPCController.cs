using UnityEngine;
using UnityEngine.AI;

public class NPCWalker : MonoBehaviour
{
    [Header("Навигация")]
    public Transform[] waypoints;
    public float waitTime = 2f;

    [Header("Анимация")]
    public Animator animator;

    private NavMeshAgent agent;
    private float waitTimer = 0f;
    private bool isWaiting = false;
    private bool isDead = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (animator == null)
        {
            Debug.LogWarning("Animator не назначен в инспекторе!");
        }

        SetWalking(true);

        if (waypoints.Length > 0)
            GoToRandomWaypoint();

        // Отключаем рэгдолл части на старте
        EnableRagdoll(false);
    }

    void Update()
    {
        if (isDead || waypoints.Length == 0) return;

        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTime;
                agent.isStopped = true;
                SetWalking(false);
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    GoToRandomWaypoint();
                    agent.isStopped = false;
                    isWaiting = false;
                    SetWalking(true);
                }
            }
        }
    }

    void GoToRandomWaypoint()
    {
        int index = Random.Range(0, waypoints.Length);
        agent.SetDestination(waypoints[index].position);
    }

    void SetWalking(bool walking)
    {
        if (animator != null)
            animator.SetBool("isWalking", walking);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("PlayerCar"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        // Отключаем контроллеры
        if (agent != null)
            agent.enabled = false;

        if (animator != null)
            animator.enabled = false;

        // Выключаем основной Rigidbody
        Rigidbody mainRb = GetComponent<Rigidbody>();
        if (mainRb != null)
            mainRb.isKinematic = true;

        // Включаем рэгдолл
        EnableRagdoll(true);

        // Удалим через 10 сек
        Destroy(gameObject, 10f);
    }

    void EnableRagdoll(bool enable)
    {
        Rigidbody[] rigidbodies = GetComponentsInChildren<Rigidbody>();
        Collider[] colliders = GetComponentsInChildren<Collider>();

        foreach (var rb in rigidbodies)
        {
            if (rb.gameObject == gameObject) continue; // пропускаем root
            rb.isKinematic = !enable;
        }


        foreach (var col in colliders)
        {
            if (col.gameObject == gameObject) continue; // пропускаем root
            col.enabled = enable;
        }

        // Отключаем основной коллайдер
        Collider mainCol = GetComponent<Collider>();
        if (mainCol != null)
            mainCol.enabled = !enable;
    }
}
