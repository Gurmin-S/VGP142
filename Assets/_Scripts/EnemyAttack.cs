using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public AudioClip swordSwing;
    public AudioClip hitSound;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    [Header("Raycast Settings")]
    public float raycastHeightOffset = 1f;

    private void Awake()
    {
        // No need for input actions anymore
    }

    public void PerformAttack()
    {
        if (!readyToAttack || attacking) return;

        readyToAttack = false;
        attacking = true;

        Invoke(nameof(ResetAttack), attackSpeed);
        Invoke(nameof(AttackRaycast), attackDelay);

        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(swordSwing);

        // Cycle between Attack1 and Attack2 triggers
        if (attackCount % 2 == 0)
        {
            animator.SetTrigger("Attack1");
        }
        else
        {
            animator.SetTrigger("Attack2");
        }

        attackCount++;
    }

    private void ResetAttack()
    {
        attacking = false;
        readyToAttack = true;
    }

    private void AttackRaycast()
    {
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        Vector3 rayDirection = transform.forward;

        Debug.DrawRay(rayOrigin, rayDirection * attackDistance, Color.red, 1f);

        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hit, attackDistance, attackLayer))
        {
            // Call HitTarget method when a hit is detected
            HitTarget(hit.point);
            Debug.Log("Hit object: " + hit.transform.name);

            if (hit.transform.TryGetComponent<Actor>(out Actor actorTarget))
            {
                Debug.Log("Damaging Actor");
                actorTarget.TakeDamage(attackDamage);
            }
            else if (hit.transform.TryGetComponent<CharacterActor>(out CharacterActor characterTarget))
            {
                Debug.Log("Damaging CharacterActor");
                characterTarget.TakeDamage(attackDamage);
            }
        }
    }

    private void HitTarget(Vector3 position)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        Gizmos.DrawLine(rayOrigin, rayOrigin + transform.forward * attackDistance);
        Gizmos.DrawSphere(rayOrigin + transform.forward * attackDistance, 0.1f);
    }
}
