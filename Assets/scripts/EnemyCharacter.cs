using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class EnemyCharacter : Character
{
    private static readonly List<EnemyCharacter> ActiveEnemies = new List<EnemyCharacter>();

    public bool addToActiveEnemies = true;

    private BehaviorGraphAgent behaviorAgent;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnEnable()
    {
        if (addToActiveEnemies && !ActiveEnemies.Contains(this))
        {
            ActiveEnemies.Add(this);
        }
    }

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
    }

    private void Start()
    {
        behaviorAgent = GetComponent<BehaviorGraphAgent>();
        behaviorAgent.BlackboardReference.SetVariableValue("Player", PlayerCharacter.Instance);
    }

    public bool TryGetSlot(out int index, out int count)
    {
        count = ActiveEnemies.Count;
        index = ActiveEnemies.IndexOf(this);
        return index >= 0 && count > 0;
    }

    public override void Die()
    {
        base.Die();

        Destroy(gameObject);
    }
}
