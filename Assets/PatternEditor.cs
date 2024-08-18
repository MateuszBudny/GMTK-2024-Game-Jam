using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices.WindowsRuntime;

[CustomEditor(typeof(BlockGrid))]
public class PatternEditor : Editor {

    public bool showLevels = true;

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        BlockGrid grid = (BlockGrid)target;

        if (grid.pattern == null || grid.pattern.Length != grid.blockSize.x * grid.blockSize.y)
        {
            grid.pattern = new BlockColors[grid.blockSize.x, grid.blockSize.y];
        }
        if (grid.patternToVerify == null || grid.patternToVerify.Length != grid.blockSize.x * grid.blockSize.y)
        {
            grid.patternToVerify = new BlockColors[grid.blockSize.x, grid.blockSize.y];
        }
        grid.pattern = Draw2DArray(grid.pattern, grid.blockSize.x, grid.blockSize.y, "Pattern");
        grid.patternToVerify = Draw2DArray(grid.patternToVerify, grid.blockSize.x, grid.blockSize.y, "Pattern to Verify");

        if (GUILayout.Button("Test")) {
            if(grid.verifyGrid(grid.start, grid.offset,grid.secondOffset, grid.patternToVerify) == null)
            {
                grid.Start();
            }            
        }

}


    public BlockColors[,] Draw2DArray(BlockColors[,] grid, int xSize = 0, int ySize = 0, string name = "")
    {
        BlockColors[,] outGrid = new BlockColors[xSize, ySize];
        EditorGUILayout.Space();

        showLevels = EditorGUILayout.Foldout(showLevels, name);
        if (showLevels)
        {
            EditorGUI.indentLevel++;

                    EditorGUI.indentLevel = 0;

                    GUIStyle tableStyle = new GUIStyle("box");
                    tableStyle.padding = new RectOffset(10, 10, 10, 10);
                    tableStyle.margin.left = 32;

                    GUIStyle headerColumnStyle = new GUIStyle();
                    headerColumnStyle.fixedWidth = 35;

                    GUIStyle columnStyle = new GUIStyle();
                    columnStyle.fixedWidth = 65;

                    GUIStyle rowStyle = new GUIStyle();
                    rowStyle.fixedHeight = 25;

                    GUIStyle rowHeaderStyle = new GUIStyle();
                    rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

                    GUIStyle columnHeaderStyle = new GUIStyle();
                    columnHeaderStyle.fixedWidth = 30;
                    columnHeaderStyle.fixedHeight = 25.5f;

                    GUIStyle columnLabelStyle = new GUIStyle();
                    columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
                    columnLabelStyle.alignment = TextAnchor.MiddleCenter;
                    columnLabelStyle.fontStyle = FontStyle.Bold;

                    GUIStyle cornerLabelStyle = new GUIStyle();
                    cornerLabelStyle.fixedWidth = 42;
                    cornerLabelStyle.alignment = TextAnchor.MiddleRight;
                    cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
                    cornerLabelStyle.fontSize = 14;
                    cornerLabelStyle.padding.top = -5;

                    GUIStyle rowLabelStyle = new GUIStyle();
                    rowLabelStyle.fixedWidth = 25;
                    rowLabelStyle.alignment = TextAnchor.MiddleRight;
                    rowLabelStyle.fontStyle = FontStyle.Bold;

                    GUIStyle enumStyle = new GUIStyle("popup");
                    rowStyle.fixedWidth = 65;

                    EditorGUILayout.BeginHorizontal(tableStyle);
                    for (int x = -1; x < xSize; x++)
                    {
                        EditorGUILayout.BeginVertical((x == -1) ? headerColumnStyle : columnStyle);
                        for (int y = -1; y < ySize; y++)
                        {
                            if (x == -1 && y == -1)
                            {
                                EditorGUILayout.BeginVertical(rowHeaderStyle);
                                EditorGUILayout.LabelField("[X,Y]", cornerLabelStyle);
                                EditorGUILayout.EndVertical();
                            }
                            else if (x == -1)
                            {
                                EditorGUILayout.BeginVertical(columnHeaderStyle);
                                EditorGUILayout.LabelField(y.ToString(), rowLabelStyle);
                                EditorGUILayout.EndVertical();
                            }
                            else if (y == -1)
                            {
                                EditorGUILayout.BeginVertical(rowHeaderStyle);
                                EditorGUILayout.LabelField(x.ToString(), columnLabelStyle);
                                EditorGUILayout.EndVertical();
                            }

                            if (x >= 0 && y >= 0)
                            {
                                EditorGUILayout.BeginHorizontal(rowStyle);
                                outGrid[x, y] = (BlockColors)EditorGUILayout.EnumPopup(grid[x, y], enumStyle);
                                EditorGUILayout.EndHorizontal();
                            }
                        }
                        EditorGUILayout.EndVertical();
                    }
                    EditorGUILayout.EndHorizontal();

        }
        return outGrid;
    }

}