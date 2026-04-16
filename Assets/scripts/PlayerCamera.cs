using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform player;
    public float smoothTime = 1f;

    private Vector3 velocity;
    private float positionY;

    private void Start()
    {
        positionY = transform.position.y;
    }

    private void Update()
    {
        var pos = Vector3.SmoothDamp(transform.position, player.position, ref velocity, Time.deltaTime * smoothTime);
        transform.position = new Vector3 (pos.x, positionY, pos.z);
    }
}
