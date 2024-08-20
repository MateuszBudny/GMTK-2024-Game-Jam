#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PatternGrid))]
public class Grid2DGizmo : Editor
{
    private void OnSceneGUI()
    {
        PatternGrid patternGrid = (PatternGrid)target;
        DrawGridGizmo(patternGrid);

    }

    private void DrawGridGizmo(PatternGrid patternGrid)
    {
        Handles.color = Color.green;

        for(int i = 0; i < patternGrid.gridSize.x + 1; i++)
        {
            Handles.DrawLine(new Vector3(i * patternGrid.cellSize.x, 0, 0) + patternGrid.transform.position, new Vector3(i * patternGrid.cellSize.x, 0, patternGrid.cellSize.y * patternGrid.gridSize.y) + patternGrid.transform.position);
        }
        for(int j = 0; j < patternGrid.gridSize.y + 1; j++)
        {
            Handles.DrawLine(new Vector3(0, 0, j * patternGrid.cellSize.y) + patternGrid.transform.position, new Vector3(patternGrid.cellSize.x * patternGrid.gridSize.x, 0, j * patternGrid.cellSize.y) + patternGrid.transform.position);
        }

    }
}
#endif