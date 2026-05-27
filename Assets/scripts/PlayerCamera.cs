using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;

    private Vector3 velocity;
    private Vector3 initialPosition;

    private void Start()
    {
        initialPosition = transform.position;
    }

    private void Update()
    {

        var targetPosition = new Vector3(player.position.x, player.position.y, player.position.z + initialPosition.z);
        var pos = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, Time.deltaTime * smoothTime);
        transform.position = new Vector3 (pos.x, initialPosition.y, pos.z);
    }
}
