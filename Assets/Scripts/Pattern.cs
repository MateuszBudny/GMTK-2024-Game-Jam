using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "ScriptableObjects/Pattern", order = 1)]
public class Pattern : ScriptableObject
{
    [SerializeField] public GameObject singleBlock;
    [SerializeField] public Vector2Int size;

    [SerializeField]
    public Grid2D<BlockColors> pattern = new (Vector2Int.zero, Vector2.zero, Vector2.zero, BlockColors.Uninitialized);

    public BlockColors GetAt(int x, int y)
    {
        return pattern.GetValueAt(x,y);
    }
    public BlockColors SetAt(int x, int y, BlockColors color)
    {
        return pattern.SetValueAt(x,y,color);
    }

    public void Resize(int x, int y)
    {
        if (pattern == null)
        {
            pattern = new Grid2D<BlockColors>(new Vector2Int(x,y),Vector2.zero, new Vector2(singleBlock.transform.localScale.x, singleBlock.transform.localScale.z),BlockColors.Uninitialized);    
        }
        if (pattern.Length == x * y)
        {
            return;
        }
        pattern = new Grid2D<BlockColors>(new Vector2Int(x,y),Vector2.zero, new Vector2(singleBlock.transform.localScale.x, singleBlock.transform.localScale.z), BlockColors.Uninitialized);
    }

    static public Dictionary<Vector2Int, GameObject> SpawnPattern(Grid2D<BlockColors> grid, GameObject singleBlock, Vector3 position, Transform parent)
    {
        Dictionary<Vector2Int, GameObject> objects = new Dictionary<Vector2Int, GameObject>();
        for (int i = 0; i < grid.gridSize.x; i++)
        {
            for (int j = 0; j < grid.gridSize.y; j++)
            {
                
                    GameObject go = Instantiate(singleBlock, position + new Vector3(i * singleBlock.transform.localScale.x, 0, j * singleBlock.transform.localScale.z), singleBlock.transform.rotation, parent);
                    go.GetComponent<CarpetSquareRenderer>().SetColors(grid.GetValueAt(i, j));
                    objects.Add(new Vector2Int(i,j), go);
                
            }
        }

        return objects;
    }
    
}
