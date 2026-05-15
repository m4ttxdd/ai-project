using System.Collections;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    public enum Size
    {
        Tiny,
        Small,
        Medium,
        Large
    }

    public Size size;

    public float health = 100f;

    [Header("Throwable Properties")]
    public float recoverDelay = 5f;
    public float groundCheckDistance = 0.5f;

    [HideInInspector] public Character thrower;

    private Coroutine RecoverCoroutine;
    private Coroutine TickDeathCoroutine;

    private LayerMask groundMask;
    private Rigidbody body;
    private NavMeshAgent agent;

    protected virtual void Awake()
    {
        body = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        groundMask = LayerMask.GetMask("Ground");
    }

    public void OnThrown()
    {
        TakeDamage(10f);

        thrower = null;

        if (RecoverCoroutine != null)
        {
            StopCoroutine(RecoverCoroutine);
        }
        RecoverCoroutine = StartCoroutine(RecoverRoutine());
    }

    private IEnumerator RecoverRoutine()
    {
        yield return new WaitForSeconds(recoverDelay);

        while (!IsGrounded())
        {
            yield return null;
        }

        if (body != null)
        {
            body.linearVelocity = Vector3.zero;
            body.angularVelocity = Vector3.zero;
            body.isKinematic = true;
        }

        if (agent != null)
        {
            agent.enabled = true;
        }
    }

    private bool IsGrounded()
    {
        var origin = transform.position + Vector3.up * 0.1f;
        return Physics.Raycast(origin, Vector3.down, groundCheckDistance + 0.1f, groundMask);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            if (TickDeathCoroutine != null)
            {
                StopCoroutine(TickDeathCoroutine);
            }
            TickDeathCoroutine = StartCoroutine(TickDeath());
        }
    }

    private IEnumerator TickDeath()
    {
        while (!IsGrounded())
        {
            yield return null;
        }
        Die();
    }

    public virtual void Die()
    {

    }
}
