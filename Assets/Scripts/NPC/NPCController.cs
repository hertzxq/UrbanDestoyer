using UnityEngine;
using UnityEngine.AI;

public class NPCWalker : MonoBehaviour
{
    public Transform[] waypoints;
    public float waitTime = 2f;
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
            Debug.LogWarning("Animator не назначен в инспекторе на " + gameObject.name);
        }

        // Устанавливаем idle в начале игры
        SetWalking(false);

        if (waypoints.Length > 0)
            GoToRandomWaypoint();
    }

    void Update()
    {
        if (isDead || waypoints.Length == 0)
            return;

        // Проверяем движение на основе скорости агента и состояния ожидания
        bool isMoving = !isWaiting && agent.velocity.magnitude > 0.1f;
        SetWalking(isMoving);
        Debug.Log($"Velocity Magnitude: {agent.velocity.magnitude}, IsWaiting: {isWaiting}, IsMoving: {isMoving}, IsWalking: {animator.GetBool("isWalking")}");

        // Логика остановки и ожидания
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            if (!isWaiting)
            {
                isWaiting = true;
                waitTimer = waitTime;
                agent.isStopped = true;
                SetWalking(false); // Явно устанавливаем idle при остановке
            }
            else
            {
                waitTimer -= Time.deltaTime;
                if (waitTimer <= 0f)
                {
                    GoToRandomWaypoint();
                    isWaiting = false;
                    agent.isStopped = false;
                }
            }
        }
    }

    void GoToRandomWaypoint()
    {
        if (waypoints.Length == 0) return;

        int index = Random.Range(0, waypoints.Length);
        // Избегаем выбора текущей позиции, если возможно
        if (waypoints.Length > 1)
        {
            Vector3 currentPosition = agent.transform.position;
            int attempts = 0;
            const int maxAttempts = 10;
            while (attempts < maxAttempts && Vector3.Distance(currentPosition, waypoints[index].position) < 0.5f)
            {
                index = Random.Range(0, waypoints.Length);
                attempts++;
            }
        }
        agent.SetDestination(waypoints[index].position);
        Debug.Log($"New destination set to: {waypoints[index].name}");
    }

    void SetWalking(bool walking)
    {
        if (animator != null)
        {
            animator.SetBool("isWalking", walking);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.enabled = false;
        SetWalking(false);

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.AddForce(Vector3.up * 4f + Random.onUnitSphere * 3f, ForceMode.Impulse);

        Destroy(gameObject, 5f);
    }
}