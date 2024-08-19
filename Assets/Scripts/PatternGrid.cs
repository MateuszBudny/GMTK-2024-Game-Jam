using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NodeCanvas.Tasks.Actions;
using Unity.VisualScripting;
using UnityEngine;

public class PatternGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize = new (100, 100);
    [SerializeField] public Vector2 cellSize = new (1, 1);
    [SerializeField] public GameObject basePattern;
    
    [SerializeField] private LineRenderer ln; 
    [SerializeField] private CarpetRender carpetRender; 
    [SerializeField] private Pattern startPattern;
    [SerializeField] private Pattern wantedPattern;

    public Stack<Grid2D<BlockColors>> oldGrid = new(); 
    
    private Collider2D collider;
    [HideInInspector] public Grid2D<BlockColors> grid;
    
    void Start()
    {
        carpetRender.gridToRender = grid;
        collider = GetComponent<Collider2D>();
        if (startPattern != null)
        {
            grid = Instantiate(startPattern).pattern;
        }

        if (grid == null || grid.Length == 0)
        {
            grid = new Grid2D<BlockColors>(gridSize, transform.position, cellSize, new BlockColors(LeafColor.Uninitialized));
        }
        
        if (startPattern?.pattern.gridSize != wantedPattern?.pattern.gridSize)
        {
            Debug.LogError("Grid size mismatch");
        }

        if (wantedPattern != null)
        {
            var objects = Pattern.SpawnPattern(wantedPattern.pattern, basePattern, grid.position, null, 0.5f);
            foreach (var obj in objects)
            {
                obj.Value.transform.position -= new Vector3(0, 10, 0);
            }
        }
        
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
        carpetRender.gridToRender = grid;
        
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
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            
            ln.SetPosition(0,-1000 * new Vector3(0, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            ln.SetPosition(1, 1000 * new Vector3(0, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(0, 1));
            }
        }
        if (Input.GetKey(KeyCode.S) )
        {
            
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            
            ln.SetPosition(0,-1000 * new Vector3(1, 0, 0) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            ln.SetPosition(1, 1000 * new Vector3(1, 0, 0) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(1, 0));
            }
        }

        if (Input.GetKey(KeyCode.Z))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            
            ln.SetPosition(0,-1000 * new Vector3(1, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            ln.SetPosition(1, 1000 * new Vector3(1, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(1, 1));
            }
        }
        if (Input.GetKey(KeyCode.X))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            
            ln.SetPosition(0,-1000 * new Vector3(-1, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            ln.SetPosition(1, 1000 * new Vector3(-1, 0, 1) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), new Vector2Int(-1, 1));
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
        
        DisableAllRenderers(!Input.GetKey(KeyCode.L));
        
        Debug.Log(isFinishedPattern());
    }

    public void showAxis(KeyCode code, Vector2Int axis, int axisLen)
    {
        if (Input.GetKey(code) )
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));
            
            ln.SetPosition(0,-axisLen * new Vector3(axis.x, 0, axis.y) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            ln.SetPosition(1, (axisLen - 1) * new Vector3(axis.x, 0, axis.y) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0,50,0));
            if (Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), axis);
            }
        }
    }

    public bool isFinishedPattern()
    {
        if (wantedPattern == null)
        {
            return true;
        }
        return Enumerable.SequenceEqual(wantedPattern.pattern, grid);
    }
    
    public void DisableAllRenderers(bool state) 
    {
        var allRenderers = gameObject.GetComponentsInChildren< Renderer >();
        foreach ( Renderer childRenderer in allRenderers )
        {
            childRenderer.enabled = state;
        }
    }
    
}
