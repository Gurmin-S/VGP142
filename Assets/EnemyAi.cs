using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform target; // Transform to assign target (player or other object)

    public LayerMask whatIsGround;
    public LayerMask attackLayer; // Make sure this is set to Hittable layer

    public float health;

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

    // Animator parameters
    public Animator animator;
    public float AnimlerpSpeed = 5f;  // Speed of the animation transition

    private Vector3 moveInput; // For moving input

    private void Awake()
    {
        if (target == null)
        {
            Debug.LogError("No target assigned to the Enemy AI.");
        }

        agent = GetComponent<NavMeshAgent>();

        if (attackScript == null)
        {
            Debug.LogError("Attack script is not assigned.");
        }

        if (animator == null)
        {
            Debug.LogError("Animator is not assigned.");
        }
    }

    private void Update()
    {
        // Update animator parameters based on AI movement state
        UpdateAnimatorParameters();

        // Check for sight and attack range
        playerInSightRange = CheckIfPlayerInRange(sightRange);
        playerInAttackRange = CheckIfPlayerInRange(attackRange);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChaseTarget();
        if (playerInAttackRange && playerInSightRange) AttackTarget();
    }

    private bool CheckIfPlayerInRange(float range)
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            if (collider.transform == target)
            {
                return true;
            }
        }
        return false;
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        // Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        // Calculate random point in range
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
        // Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        if (target != null)
        {
            transform.LookAt(target);
        }

        if (!alreadyAttacked)
        {
            // Perform melee attack via Attack script
            PerformMeleeAttack();

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void PerformMeleeAttack()
    {
        if (attackScript != null)
        {
            attackScript.PerformAttack(); // Use the existing attack script to perform the attack
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    // Update the Animator parameters based on movement state
    private void UpdateAnimatorParameters()
    {
        if (animator != null)
        {
            // Calculate movement speed (normalized)
            float targetSpeed = agent.velocity.sqrMagnitude > 0 ? (agent.velocity.magnitude > 1.5f ? 1f : 0.5f) : 0f;

            // Smoothly transition to the target speed
            float smoothSpeed = Mathf.Lerp(animator.GetFloat("Speed"), targetSpeed, Time.deltaTime * AnimlerpSpeed);

            // Set the new smoothed speed to the animator
            animator.SetFloat("Speed", smoothSpeed);

            // Update the isGrounded parameter based on agent's velocity
            animator.SetBool("isGrounded", agent.isOnNavMesh); // or use your own method to determine grounded status
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
