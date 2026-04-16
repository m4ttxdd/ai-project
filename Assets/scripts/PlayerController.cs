using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public LayerMask groundMask;

    private Camera cam;
    private Mover mover;

    private void Start()
    {
        mover = GetComponent<Mover>();
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            var mouseRay = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast(cam.transform.position, mouseRay.direction, out hit, 100, groundMask))
            {
                mover.Move(hit.point);
            }
        }

        if (Input.GetKeyDown(KeyCode.S)) 
        {
            mover.Stop();
        }
    }

}
