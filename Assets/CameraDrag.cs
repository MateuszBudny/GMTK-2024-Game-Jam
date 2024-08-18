using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 1;
    private Vector3 dragOrigin;


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
        Vector3 move = new Vector3(pos.x * Camera.main.aspect, 0, pos.y) * Camera.main.orthographicSize * dragSpeed;
        dragOrigin = Input.mousePosition;

        transform.Translate(-move, Space.World);
    }
}