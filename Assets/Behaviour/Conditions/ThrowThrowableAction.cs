using System;
using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using Random = UnityEngine.Random;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ThrowThrowable", story: "[Agent] throws [Throwable] at [Target]", category: "Action", id: "e48e0c5a46785141a5799499fb34cd13")]
public partial class ThrowThrowableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Throwable;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Transform> HoldPoint;

    private float flightTime = 0.6f;
    private float enemySearchRadius = 25f;
    private float minRandomThrowDistance = 8f;
    private float maxRandomThrowDistance = 18f;
    private float randomPointSampleRadius = 2f;

    private Character throwableCharacter;

    protected override Status OnStart()
    {
        if (Throwable.Value == null || Agent.Value == null || flightTime <= 0f)
        {
            return Status.Failure;
        }

        var origin = HoldPoint.Value != null ? HoldPoint.Value.position : Throwable.Value.transform.position;

        if (!TryGetTargetPosition(origin, out var targetPosition))
        {
            return Status.Failure;
        }

        var body = Throwable.Value.GetComponent<Rigidbody>();
        if (body == null)
        {
            return Status.Failure;
        }

        var displacement = targetPosition - origin;
        var launchVelocity = (displacement - 0.5f * Physics.gravity * flightTime * flightTime) / flightTime;

        Throwable.Value.transform.SetParent(null);

        body.isKinematic = false;
        body.linearVelocity = launchVelocity;

        throwableCharacter = Throwable.Value.GetComponent<Character>();
        if (throwableCharacter != null)
        {
            throwableCharacter.OnThrown();
        }

        Throwable.Value = null;
        return Status.Success;
    }

    private bool TryGetTargetPosition(Vector3 origin, out Vector3 targetPosition)
    {
        if (Target.Value != null)
        {
            targetPosition = GetPredictedPosition(Target.Value);
            return true;
        }

        if (TryGetNearestEnemy(out var enemy))
        {
            targetPosition = GetPredictedPosition(enemy);
            return true;
        }

        return TryGetRandomThrowPoint(origin, out targetPosition);
    }

    private Vector3 GetPredictedPosition(GameObject target)
    {
        var position = target.transform.position;

        if (TryGetTargetVelocity(target, out var velocity))
        {
            position += velocity * flightTime;
        }

        return position;
    }

    private bool TryGetTargetVelocity(GameObject target, out Vector3 velocity)
    {
        velocity = Vector3.zero;

        if (target == null)
        {
            return false;
        }

        if (target.TryGetComponent(out NavMeshAgent navAgent) && navAgent.enabled)
        {
            velocity = navAgent.velocity;
            return true;
        }

        if (target.TryGetComponent(out Rigidbody body) && !body.isKinematic)
        {
            velocity = body.linearVelocity;
            return true;
        }

        return false;
    }

    private bool TryGetNearestEnemy(out GameObject enemy)
    {
        enemy = null;

        var hits = Physics.OverlapSphere(Agent.Value.transform.position, enemySearchRadius, LayerMask.GetMask("Enemy"));
        var bestDistance = float.MaxValue;
        var bestSize = int.MinValue;

        foreach (var hit in hits)
        {
            var enemyCharacter = hit.GetComponentInParent<EnemyCharacter>();
            if (enemyCharacter == null || !enemyCharacter.addToActiveEnemies)
            {
                continue;
            }

            var candidate = enemyCharacter.gameObject;
            if (candidate == Agent.Value)
            {
                continue;
            }

            var sizeValue = (int)enemyCharacter.size;
            var distance = Vector3.Distance(Agent.Value.transform.position, candidate.transform.position);

            if (sizeValue > bestSize || (sizeValue == bestSize && distance < bestDistance))
            {
                bestSize = sizeValue;
                bestDistance = distance;
                enemy = candidate;
            }
        }

        return enemy != null;
    }

    private bool TryGetRandomThrowPoint(Vector3 origin, out Vector3 targetPosition)
    {
        targetPosition = Vector3.zero;

        var direction2D = Random.insideUnitCircle.normalized;
        var distance = Random.Range(minRandomThrowDistance, maxRandomThrowDistance);
        var candidate = Agent.Value.transform.position + new Vector3(direction2D.x, 0f, direction2D.y) * distance;

        if (NavMesh.SamplePosition(candidate, out var hit, randomPointSampleRadius, NavMesh.AllAreas))
        {
            targetPosition = hit.position;
            return true;
        }

        targetPosition = candidate;
        return true;
    }
}

