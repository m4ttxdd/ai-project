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
        agent.SetDestination(position);
    }

    public void Stop()
    {
        agent.SetDestination(transform.position);
    }
}
