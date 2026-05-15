using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Explode", story: "[Agent] explodes dealing [Damage] around it", category: "Action", id: "c5aaa7b6b30aef5efd38dfc086d96209")]
public partial class ExplodeAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<float> Damage;

    protected override Status OnStart()
    {
        var mask = LayerMask.GetMask("Player", "Enemy");

        var hits = Physics.OverlapSphere(Agent.Value.transform.position, 2.5f, mask);
        
        foreach (var hit in hits)
        {
            if (hit.TryGetComponent(out Character character))
            {
                character.TakeDamage(Damage.Value);
            }
        }

        Agent.Value.GetComponentInParent<Character>().Die();

        return Status.Success;
    }
}

