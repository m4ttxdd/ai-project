using Unity.Behavior;
using UnityEngine;

public class EnemyCharacter : Character
{
    private BehaviorGraphAgent behaviorAgent;

    protected override void Awake()
    {
        base.Awake();
        
    }

    private void Start()
    {
        
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        behaviorAgent.BlackboardReference.SetVariableValue("Player", PlayerCharacter.Instance);
    }
    private void Update()
    {
        //behaviorAgent.BlackboardReference.SetVariableValue("Player", GameObject.FindGameObjectWithTag("Player").GetComponent<Character>());
    }

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
