using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "HasThrower", story: "[Agent] has [Thrower]", category: "Conditions", id: "0540c03260c19689b7d86f1b145ce13a")]
public partial class HasThrowerCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Character> Thrower;

    public override bool IsTrue()
    {
        Thrower.Value = null;

        if (Agent.Value == null)
        {
            return false;
        }

        var character = Agent.Value.GetComponent<Character>();
        if (character == null)
        {
            return false;
        }

        if (character.thrower == null)
        {
            return false;
        }

        Thrower.Value = character.thrower;
        return true;
    }

    public override void OnStart()
    {
    }

    public override void OnEnd()
    {
    }
}
