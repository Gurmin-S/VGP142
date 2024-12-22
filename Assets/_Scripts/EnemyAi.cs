using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public NavMeshAgent agent;

    public LayerMask whatIsGround;
    public LayerMask attackLayer;

    // Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    // Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;

    public EnemyAttack attackScript; // Reference to the Attack script

    // States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    public Animator animator;
    public float AnimlerpSpeed = 5f; // Speed of the animation transition

    private Transform target;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (attackScript == null)
        {
            Debug.LogError("Attack script is not assigned.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
        }

        // Automatically find the first player object with the "Player" tag
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
        else
        {
            Debug.LogError("No object with the 'Player' tag found!");
        }
    }

    private void Update()
    {
        UpdateAnimatorParameters();

        // Check for sight and attack range
        playerInSightRange = CheckIfInRange(target, sightRange);
        playerInAttackRange = CheckIfInRange(target, attackRange);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChaseTarget();
        if (playerInAttackRange && playerInSightRange) AttackTarget();
    }

    private bool CheckIfInRange(Transform target, float range)
    {
        if (target == null) return false;

        float distance = Vector3.Distance(transform.position, target.position);
        return distance <= range;
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
            walkPointSet = true;
    }

    private void ChaseTarget()
    {
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void AttackTarget()
    {
        agent.SetDestination(transform.position);

        if (target != null)
        {
            transform.LookAt(target);
        }

        if (!alreadyAttacked)
        {
            PerformMeleeAttack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PerformMeleeAttack()
    {
        if (attackScript != null)
        {
            attackScript.PerformAttack();
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void UpdateAnimatorParameters()
    {
        if (animator != null)
        {
            float targetSpeed = agent.velocity.sqrMagnitude > 0 ? (agent.velocity.magnitude > 1.5f ? 1f : 0.5f) : 0f;
            float smoothSpeed = Mathf.Lerp(animator.GetFloat("Speed"), targetSpeed, Time.deltaTime * AnimlerpSpeed);
            animator.SetFloat("Speed", smoothSpeed);
            animator.SetBool("isGrounded", agent.isOnNavMesh);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
