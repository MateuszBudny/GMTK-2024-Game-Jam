using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Runtime.InteropServices.WindowsRuntime;
using Sirenix.Utilities;

[CustomEditor(typeof(Pattern))]
public class PatternEditor : Editor {
    
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        
        Pattern pattern = (Pattern)target;
        
            pattern.Resize(pattern.size.x, pattern.size.y);
       
        Draw2DArray(pattern, pattern.size.x, pattern.size.y, "Pattern");
       
        serializedObject.ApplyModifiedProperties();

    }


    public void Draw2DArray(Pattern grid, int xSize = 0, int ySize = 0, string name = "")
    {
        EditorGUILayout.Space();

        EditorGUI.indentLevel = 0;

        var tableStyle = new GUIStyle("box");
        tableStyle.padding = new RectOffset(10, 10, 10, 10);
        tableStyle.margin.left = 32;

        var headerColumnStyle = new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        var columnStyle = new GUIStyle();
        columnStyle.fixedWidth = 65;

        var rowStyle = new GUIStyle();
        rowStyle.fixedHeight = 25;

        var rowHeaderStyle = new GUIStyle();
        rowHeaderStyle.fixedWidth = columnStyle.fixedWidth - 1;

        var columnHeaderStyle = new GUIStyle();
        columnHeaderStyle.fixedWidth = 30;
        columnHeaderStyle.fixedHeight = 25.5f;

        var columnLabelStyle = new GUIStyle();
        columnLabelStyle.fixedWidth = rowHeaderStyle.fixedWidth - 6;
        columnLabelStyle.alignment = TextAnchor.MiddleCenter;
        columnLabelStyle.fontStyle = FontStyle.Bold;

        var cornerLabelStyle = new GUIStyle();
        cornerLabelStyle.fixedWidth = 42;
        cornerLabelStyle.alignment = TextAnchor.MiddleRight;
        cornerLabelStyle.fontStyle = FontStyle.BoldAndItalic;
        cornerLabelStyle.fontSize = 14;
        cornerLabelStyle.padding.top = -5;

        var rowLabelStyle = new GUIStyle();
        rowLabelStyle.fixedWidth = 25;
        rowLabelStyle.alignment = TextAnchor.MiddleRight;
        rowLabelStyle.fontStyle = FontStyle.Bold;

        var enumStyle = new GUIStyle("popup");
        rowStyle.fixedWidth = 65;

        EditorGUILayout.BeginHorizontal(tableStyle);
        for (var x = -1; x < xSize; x++)
        {
            EditorGUILayout.BeginVertical(x == -1 ? headerColumnStyle : columnStyle);
            for (var y = -1; y < ySize; y++)
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

                    var newColor = (BlockColors)EditorGUILayout.EnumPopup(grid.GetAt(x,y), enumStyle);
                    if (newColor != grid.GetAt(x,y))
                    {
                        grid.SetAt(x, y, newColor);
                        EditorUtility.SetDirty(this);
                    }

                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();
        
    }

}