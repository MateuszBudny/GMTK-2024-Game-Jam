using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class CameraDrag : MonoBehaviour
{

    [SerializeField] public Camera dragCamera;
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private float zoomSpeed = 1;
    [SerializeField] private Canvas canvas;
    private Vector3 dragOrigin;
    private bool dragging = false;

    void Update()
    {
        dragCamera.orthographicSize -= zoomSpeed * Input.mouseScrollDelta.y;
        canvas.transform.localScale = new Vector3(dragCamera.orthographicSize/canvas.pixelRect.height, dragCamera.orthographicSize/canvas.pixelRect.height, 1) * 2;

        if (dragging)
        {
            Vector3 pos = dragCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragCamera.aspect, 0, pos.y) * dragCamera.orthographicSize * dragSpeed;
            dragOrigin = Input.mousePosition;

            transform.Translate(-move, Space.World);
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }
        
    }

    public void StartDrag()
    {
            dragging = true;
            dragOrigin = Input.mousePosition;
    }

}