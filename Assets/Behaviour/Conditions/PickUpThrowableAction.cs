using System;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "PickUpThrowable", story: "[Agent] picks up [Throwable] at [HoldPoint]", category: "Action", id: "4a47657b2ef250e877769d4fdc7a48af")]
public partial class PickUpThrowableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Throwable;
    [SerializeReference] public BlackboardVariable<Transform> HoldPoint;
    protected override Status OnStart()
    {
        if (Agent.Value == null || Throwable.Value == null || HoldPoint.Value == null) 
        {
            return Status.Failure;
        }

        var body = Throwable.Value.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.isKinematic = true;
            //body.linearVelocity = Vector3.zero;
        }

        var navAgent = Throwable.Value.GetComponent<NavMeshAgent>();
        if (navAgent != null)
        {
            navAgent.enabled = false;
        }

        Throwable.Value.transform.SetParent(HoldPoint.Value);
        Throwable.Value.transform.localPosition = Vector3.zero;
        Throwable.Value.transform.localRotation = Quaternion.identity;

        return Status.Success;
    }
}

