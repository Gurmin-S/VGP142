using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    public Animator animator;
    public AudioSource audioSource;

    [Header("Attacking")]
    public float attackDistance = 3f;
    public float attackDelay = 0.4f;
    public float attackSpeed = 1f;
    public int attackDamage = 1;
    public LayerMask attackLayer;

    public GameObject hitEffect;
    public AudioClip swordSwing;
    public AudioClip hitSound;

    private bool attacking = false;
    private bool readyToAttack = true;
    private int attackCount;

    private Inputs inputActions;

    public const string ATTACK1 = "Attack1";
    public const string ATTACK2 = "Attack2";

    [Header("Raycast Settings")]
    public float raycastHeightOffset = 1f;

    private void Awake()
    {
        inputActions = new Inputs();
    }

    private void OnEnable()
    {
        inputActions.controls.Enable();
        inputActions.controls.Attack.performed += OnAttack;
    }

    private void OnDisable()
    {
        inputActions.controls.Attack.performed -= OnAttack;
        inputActions.controls.Disable();
    }

    private void OnAttack(InputAction.CallbackContext context)
    {
        PerformAttack();
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
            animator.SetTrigger(ATTACK1);
        }
        else
        {
            animator.SetTrigger(ATTACK2);
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
            HitTarget(hit.point);

            if (hit.transform.TryGetComponent<Actor>(out Actor target))
            {
                target.TakeDamage(attackDamage);
            }
        }
    }

    private void HitTarget(Vector3 position)
    {
        audioSource.pitch = 1;
        audioSource.PlayOneShot(hitSound);

        GameObject hitInstance = Instantiate(hitEffect, position, Quaternion.identity);
        Destroy(hitInstance, 20);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 rayOrigin = transform.position + Vector3.up * raycastHeightOffset;
        Gizmos.DrawLine(rayOrigin, rayOrigin + transform.forward * attackDistance);
        Gizmos.DrawSphere(rayOrigin + transform.forward * attackDistance, 0.1f);
    }
}
