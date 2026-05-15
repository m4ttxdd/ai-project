using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "AcquireThrowable", story: "[Agent] acquires [Throwable]", category: "Conditions", id: "5602096bc86543e0a357dd7186651fa7")]
public partial class AcquireThrowableCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Throwable;

    float searchRadius = 20f;

    public override bool IsTrue()
    {
        if (Agent.Value == null)
        {
            Throwable.Value = null;
            return false;
        }


        var agentCharacter = Agent.Value.GetComponent<Character>();
        if (agentCharacter == null)
        {
            Throwable.Value = null;
            return false;
        }

        LayerMask throwableMask = LayerMask.GetMask("Enemy", "Player");
        var hits = Physics.OverlapSphere(Agent.Value.transform.position, searchRadius, throwableMask);
        var bestDistance = float.MaxValue;
        Character best = null;

        foreach (var hit in hits)
        {
            var candidate = hit.GetComponentInParent<Character>();
            if (candidate == null || candidate == agentCharacter)
            {
                continue;
            }

            if (candidate.size >= agentCharacter.size)
            {
                continue;
            }

            if (candidate.thrower != null)
            {
                continue;
            }

            var body = candidate.GetComponent<Rigidbody>();
            if (body == null || !body.isKinematic)
            {
                continue;
            }

            var distance = Vector3.Distance(Agent.Value.transform.position, candidate.transform.position);
            if (distance < bestDistance)
            {
                bestDistance = distance;
                best = candidate;
            }
        }

        if(best == null)
        {
            Throwable.Value = null;
            return false;
        }

        Throwable.Value = best;
        Throwable.Value.thrower = agentCharacter;
        return best != null;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
