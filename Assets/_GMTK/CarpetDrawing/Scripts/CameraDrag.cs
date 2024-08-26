using UnityEngine;

public class CameraDrag : MonoBehaviour
{

    [SerializeField] public Camera dragCamera;
    [SerializeField] private float dragSpeed = 1;
    [SerializeField] private float zoomSpeed = 1;
    [SerializeField] private Canvas canvas;

    [SerializeField] private PatternGrid patternGrid;
    [SerializeField] private Vector2 cameraSize = new Vector2(2, 100);

    private Vector3 dragOrigin;
    private bool dragging = false;

    private void Start()
    {
        Vector2 size = Vector2.Scale(patternGrid.grid.gridSize, patternGrid.grid.cellSize);
        Vector3 position = new Vector3(size.x / 2, 100, size.y / 2);

        dragCamera.transform.position = position;
        dragCamera.orthographicSize = size.x / 2;
    }

    void Update()
    {
        dragCamera.orthographicSize -= zoomSpeed * Input.mouseScrollDelta.y;
        dragCamera.orthographicSize = Mathf.Clamp(dragCamera.orthographicSize, cameraSize.x, cameraSize.y);
        canvas.transform.localScale = new Vector3(dragCamera.orthographicSize / canvas.pixelRect.height, dragCamera.orthographicSize / canvas.pixelRect.height, 1) * 2;

        if(dragging)
        {
            Vector3 pos = dragCamera.ScreenToViewportPoint(Input.mousePosition - dragOrigin);
            Vector3 move = new Vector3(pos.x * dragCamera.aspect, 0, pos.y) * dragCamera.orthographicSize * dragSpeed;
            dragOrigin = Input.mousePosition;

            transform.Translate(-move, Space.World);
        }

        if(Input.GetMouseButtonUp(0))
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