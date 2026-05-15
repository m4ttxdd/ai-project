using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTarget", story: "[Agent] navigates to [Target] (Reevaluates every tick)", category: "Action", id: "a44f6535c1e5b1ccb50786bdcb8cd067")]
public partial class MoveToTargetAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Target;
    
    private Mover mover;

    protected override Status OnStart()
    {
        return Tick();
    }

    protected override Status OnUpdate()
    {
        return Tick();
    }

    private Status Tick()
    {
        if (Target.Value == null)
        {
            //Debug.LogWarning("MoveToTargetAction: Target is null");
            return Status.Failure;
        }

        if (mover == null && !TryGetMover(out mover))
        {
            return Status.Failure;
        }

        mover.Move(Target.Value.transform.position);
        return Status.Success;
    }

    private bool TryGetMover(out Mover foundMover)
    {
        foundMover = null;

        if (Agent.Value == null)
        {
            return false;
        }

        foundMover = Agent.Value.GetComponent<Mover>();
        return foundMover != null;
    }
}

