using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ThrowThrowable", story: "[Agent] throws [Throwable] at [Target]", category: "Action", id: "e48e0c5a46785141a5799499fb34cd13")]
public partial class ThrowThrowableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Throwable;
    [SerializeReference] public BlackboardVariable<GameObject> Target;
    [SerializeReference] public BlackboardVariable<Transform> HoldPoint;

    public float throwSpeed = 6f;
    public float throwArc = 6f;
    private Character throwableCharacter;

    protected override Status OnStart()
    {
        if (Throwable.Value == null || Target.Value == null)
        {
            return Status.Failure;
        }

        var origin = HoldPoint.Value != null ? HoldPoint.Value.position : Throwable.Value.transform.position;
        var direction = Target.Value.transform.position - origin;
        direction.y = 0f;

        var body = Throwable.Value.GetComponent<Rigidbody>();
        if (body == null)
        {
            return Status.Failure;
        }

        Throwable.Value.transform.SetParent(null);

        body.isKinematic = false;
        body.linearVelocity = direction.normalized * throwSpeed + Vector3.up * throwArc;

        throwableCharacter = Throwable.Value.GetComponent<Character>();
        if (throwableCharacter != null)
        {
            throwableCharacter.OnThrown();
        }

        Throwable.Value = null;
        return Status.Success;
    }
}

