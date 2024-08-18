using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize = new (100, 100);
    [SerializeField] public Vector2 cellSize = new (1, 1);

    private Collider2D collider;
    [HideInInspector] public Grid2D<BlockColors> grid;
    void Start()
    {
        collider = GetComponent<Collider2D>();
        grid = new Grid2D<BlockColors>(gridSize, transform.position, cellSize, new BlockColors(LeafColor.White));
        
    }

    public void AddPattern(Vector3 position, Pattern pattern)
    {
        AddPattern(grid.GetClosestGridPosition(new Vector2(position.x,position.z)),pattern);
    }
    
    public void AddPattern(Vector2Int position, Pattern pattern)
    {
        foreach (var patternSquare in pattern.pattern)
        {
            if (!patternSquare.value.Equals(BlockColors.Uninitialized))
            {
                BlockColors newBlock = BlockColors.Add(grid.GetValueAt(position + patternSquare.gridPosition),patternSquare.value);
                grid.SetValueAt(position + patternSquare.gridPosition, newBlock);
            }
        }
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        Pattern.SpawnPattern(grid, pattern.singleBlock, grid.position, transform);

    }
    
}
