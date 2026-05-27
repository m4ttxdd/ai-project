using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private CharacterController characterController;
    private NavMeshAgent agent;

    [Header("Separation")]
    public bool useSeparation = false;
    public float separationRadius = 2.0f;
    public float separationDisableDistance = 1.5f;//stop seperation here
    public float separationFullDistance = 8.0f; //distance where seperation is full
    public float repathInterval = 0.2f;

    private float lastRepathTime;
    private EnemyCharacter enemyCharacter;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        enemyCharacter = GetComponent<EnemyCharacter>();
    }

    public void Move(Vector3 position)
    {
        if (!agent.enabled) return;

        if (Time.time - lastRepathTime < repathInterval)
        {
            return;
        }

        lastRepathTime = Time.time;

        var destination = position;

        if (useSeparation && separationRadius > 0.0f && enemyCharacter != null)
        {
            var distanceToTarget = Vector3.Distance(transform.position, position);

            if (distanceToTarget > separationDisableDistance)//dont disable serparte if far
            {
                if (enemyCharacter.TryGetSlot(out var index, out var count))
                {
                    //scale down the seperation radius when enemy gets closer
                    var t = Mathf.Clamp01((distanceToTarget - separationDisableDistance) / (separationFullDistance - separationDisableDistance));
                    var scaledRadius = separationRadius * t;

                    //find the destination on circle around player
                    var angle = count > 1 ? (360.0f / count) * index : 0.0f;
                    var radians = angle * Mathf.Deg2Rad;
                    var offset = new Vector3(Mathf.Cos(radians), 0.0f, Mathf.Sin(radians)) * scaledRadius;

                    var candidate = position + offset;

                    if (NavMesh.SamplePosition(candidate, out var hit, scaledRadius, NavMesh.AllAreas))
                    {
                        destination = hit.position;
                    }
                    else
                    {
                        destination = candidate;
                    }
                }
            }
        }

        agent.SetDestination(destination);
    }

    public void Stop()
    {
        if (!agent.enabled) return;

        agent.SetDestination(transform.position);
    }
}
