using UnityEngine;

public class CarpetRender : MonoBehaviour
{
    [SerializeField] public PatternGrid gridToRender;
    [SerializeField] private RenderTexture renderTexture;

    public Texture2D toTexture2D()
    {
        Vector2 size = Vector2.Scale(gridToRender.grid.gridSize, gridToRender.grid.cellSize);
        Vector3 position = new Vector3(size.x / 2, 100, size.y / 2);
        Camera camera = GetComponent<Camera>();
        camera.transform.position = position;
        camera.orthographicSize = size.x / 2;
        camera.aspect = size.y / size.x;
        camera.Render();

        Texture2D tex = new Texture2D(512, 512, TextureFormat.RGB24, false);
        // ReadPixels looks at the active RenderTexture.
        RenderTexture.active = camera.activeTexture;
        tex.ReadPixels(new Rect(0, 0, camera.activeTexture.width, camera.activeTexture.height), 0, 0);
        tex.Apply();
        return tex;
    }

}
