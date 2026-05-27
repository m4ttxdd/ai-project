using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Spawn Prefab as Throwable", story: "[Agent] spawns [Prefab] at [Holdpoint] as [Throwable]", category: "Action", id: "dbd53e65837caa1c479fcdb184746b8a")]
public partial class SpawnPrefabAsThrowableAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> Prefab;
    [SerializeReference] public BlackboardVariable<Transform> Holdpoint;
    [SerializeReference] public BlackboardVariable<Character> Throwable;

    protected override Status OnStart()
    {
        if (Agent.Value == null || Prefab.Value == null || Holdpoint.Value == null || Throwable.Value != null)
        {
            return Status.Failure;
        }

        var instance = UnityEngine.Object.Instantiate(Prefab.Value, Holdpoint.Value.position, Holdpoint.Value.rotation);
        instance.transform.SetParent(Holdpoint.Value, true);

        if (!instance.TryGetComponent(out Character character))
        {
            UnityEngine.Object.Destroy(instance);
            return Status.Failure;
        }

        var body = instance.GetComponent<Rigidbody>();
        if (body != null)
        {
            body.isKinematic = true;
        }

        Throwable.Value = character;
        return Status.Success;
    }
}

