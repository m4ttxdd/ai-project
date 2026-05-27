using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationPerSecond = new Vector3(0, 45, 0);

    void Update()
    {
        transform.Rotate(rotationPerSecond * Time.deltaTime);
    }
}
