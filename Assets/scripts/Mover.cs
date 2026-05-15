using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(CharacterController))]
[RequireComponent (typeof(NavMeshAgent))]
public class Mover : MonoBehaviour
{
    private CharacterController characterController;
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    public void Move(Vector3 position)
    {
        if (!agent.enabled) return;

        agent.SetDestination(position);
    }

    public void Stop()
    {
        if (!agent.enabled) return;

        agent.SetDestination(transform.position);
    }
}
