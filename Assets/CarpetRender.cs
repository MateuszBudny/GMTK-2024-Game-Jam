using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetRender : MonoBehaviour
{
    [SerializeField]public Grid2D<BlockColors> gridToRender;
    [SerializeField]private RenderTexture renderTexture;
    [SerializeField]private Camera camera;
    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gridToRender != null)
        {
        Vector2 size = Vector2.Scale(gridToRender.gridSize, gridToRender.cellSize);
        camera.orthographicSize = size.x;
        camera.aspect = size.y / size.x;
        }
    }
}
