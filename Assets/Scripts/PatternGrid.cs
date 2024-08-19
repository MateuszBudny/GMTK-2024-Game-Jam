using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize = new (100, 100);
    [SerializeField] public Vector2 cellSize = new (1, 1);
    [SerializeField] public GameObject basePattern;

    
    Stack<Grid2D<BlockColors>> oldGrid = new(); 
    
    private Collider2D collider;
    [HideInInspector] public Grid2D<BlockColors> grid;
    void Start()
    {
        collider = GetComponent<Collider2D>();
        grid = new Grid2D<BlockColors>(gridSize, transform.position, cellSize, new BlockColors(LeafColor.Uninitialized));
        Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
    }

    public void AddPattern(Vector3 position, Pattern pattern)
    {
        AddPattern(grid.GetClosestGridPosition(new Vector2(position.x,position.z)),pattern);
    }
    
    public void AddPattern(Vector2Int position, Pattern pattern)
    {
        oldGrid.Push(grid.Clone() as Grid2D<BlockColors>);
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

    public void MirrorPattern(Vector2Int position, Vector2Int axis)
    {
        oldGrid.Push(grid.Clone() as Grid2D<BlockColors>);
        var tempGrid = (Grid2D<BlockColors>)grid.Clone();
        foreach (var gridSquare in grid)
        {
            if(!gridSquare.value.Equals(BlockColors.Uninitialized))
            {
                Vector2Int mirrorPosition = (gridSquare.gridPosition - position);
                if( axis == new Vector2Int(0,1) ||  axis == new Vector2Int(0,-1))
                {
                    mirrorPosition.x *= -1;
                    mirrorPosition.x -= 1;
                    mirrorPosition += position;
                }
                else if (axis == new Vector2Int(1, 0) || axis == new Vector2Int(-1, 0))
                {
                    mirrorPosition.y *= -1;
                    mirrorPosition.y -= 1;
                    mirrorPosition += position;
                }
                if( axis == new Vector2Int(1,1) ||  axis == new Vector2Int(-1,-1))
                {
                    int delta = mirrorPosition.x - mirrorPosition.y;
                    mirrorPosition = gridSquare.gridPosition + delta * new Vector2Int(-1, 1);
                }
                else if (axis == new Vector2Int(1, -1) || axis == new Vector2Int(-1, 1))
                {
                    int delta = -(mirrorPosition.y + mirrorPosition.x) - 1;
                    
                    mirrorPosition = gridSquare.gridPosition + delta * new Vector2Int(1, 1);
                }
                
                if (grid.IsInsideGrid(mirrorPosition))
                {
                    BlockColors newValue = BlockColors.Add(grid.GetValueAt(mirrorPosition),MirrorSquare(gridSquare.value, axis));
                    tempGrid.SetValueAt(mirrorPosition, newValue);
                }

            }
            
        }

        grid = tempGrid;
        
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
        
    }

    private BlockColors MirrorSquare(BlockColors square, Vector2Int axis)
    {
        if( axis == new Vector2Int(0,1) || axis == new Vector2Int(0,-1))
        {
            return new BlockColors(square.colors[2], square.colors[1], square.colors[0], square.colors[3]);
        }
        else if (axis == new Vector2Int(1, 0) || axis == new Vector2Int(-1, 0))
        {
            return new BlockColors(square.colors[0], square.colors[3], square.colors[2], square.colors[1]);
        }
        else if (axis == new Vector2Int(1, 1) || axis == new Vector2Int(-1, -1))
        {
            return new BlockColors(square.colors[1], square.colors[0], square.colors[3], square.colors[2]);
        }
        else if (axis == new Vector2Int(-1, 1) || axis == new Vector2Int(1, -1))
        {
            return new BlockColors(square.colors[3], square.colors[2], square.colors[1], square.colors[0]);
        }
        return square;
    }


    public void Update()
    {
        if (Input.GetKey(KeyCode.A) )
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(0, 1));
            }
        }
        if (Input.GetKey(KeyCode.S) )
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(1, 0));
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(1, 1));
            }
        }
        if (Input.GetKey(KeyCode.X))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(-1, 1));
            }
        }
        
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(JsonUtility.ToJson(grid));
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            oldGrid = new Stack<Grid2D<BlockColors>>();
            grid = new Grid2D<BlockColors>(grid.gridSize, grid.position, grid.cellSize, BlockColors.Uninitialized);
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }

            Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (oldGrid.Count > 0)
            {
                var backGrid = oldGrid.Pop();
                if (backGrid != null)
                {
                    grid = backGrid;
                    foreach (Transform child in transform) {
                        Destroy(child.gameObject);
                    }

                    Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
                }    
            }
            
        }
    }
    
}
