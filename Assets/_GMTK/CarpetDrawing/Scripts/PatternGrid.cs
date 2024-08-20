using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PatternGrid : MonoBehaviour
{
    [SerializeField] public Vector2Int gridSize = new(100, 100);
    [SerializeField] public Vector2 cellSize = new(1, 1);
    [SerializeField] public GameObject basePattern;

    [SerializeField] private LineRenderer ln;
    [SerializeField] private CarpetRender carpetRender;
    [SerializeField] private Pattern startPattern;
    [SerializeField] private Pattern wantedPattern;
    [SerializeField] private Transform patternFinish;

    [SerializeField] public GameObject arrow;

    public Stack<Grid2D<BlockColors>> oldGrid = new();

    private new Collider2D collider;
    [HideInInspector] public Grid2D<BlockColors> grid;


    private void Awake()
    {
        if(CursorManager.Instance != null)
        {

            CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.None;
        }
        if(DrawingBridge.Instance?.startPattern != null)
        {
            startPattern = DrawingBridge.Instance.startPattern;

        }
        if(DrawingBridge.Instance?.wantedPattern != null)
        {
            wantedPattern = DrawingBridge.Instance.wantedPattern;
        }
    }

    void Start()
    {
        collider = GetComponent<Collider2D>();
        if(startPattern != null)
        {
            grid = Instantiate(startPattern).pattern;
        }

        if(grid == null || grid.Length == 0)
        {
            grid = new Grid2D<BlockColors>(gridSize, transform.position, cellSize, new BlockColors(LeafColor.Uninitialized));
        }

        if(startPattern?.pattern.gridSize != wantedPattern?.pattern.gridSize)
        {
            Debug.LogError("Grid size mismatch");
        }

        if(wantedPattern != null)
        {
            var objects = Pattern.SpawnPattern(wantedPattern.pattern, basePattern, grid.position, patternFinish, 0.2f);
            foreach(var obj in objects)
            {
                obj.Value.transform.position -= new Vector3(0, 10, 0);
            }
        }

        Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
    }

    public void AddPattern(Vector3 position, Pattern pattern)
    {
        Vector2Int gridPosition = grid.GetClosestGridPosition(new Vector2(position.x, position.z));
        if(grid.IsInsideGrid(gridPosition))
        {
            AddPattern(gridPosition, pattern);
        }
    }

    public void AddPattern(Vector2Int position, Pattern pattern)
    {
        oldGrid.Push(grid.Clone() as Grid2D<BlockColors>);
        foreach(var patternSquare in pattern.pattern)
        {
            if(!patternSquare.value.Equals(BlockColors.Uninitialized))
            {
                BlockColors newBlock = BlockColors.Add(grid.GetValueAt(position + patternSquare.gridPosition), patternSquare.value);
                grid.SetValueAt(position + patternSquare.gridPosition, newBlock);
            }
        }
        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Pattern.SpawnPattern(grid, pattern.singleBlock, grid.position, transform);

        SoundManager.Instance.Play(Audio.SinglePatternPutOnGrid);
    }

    public void MirrorPattern(Vector2Int position, Vector2Int axis)
    {
        oldGrid.Push(grid.Clone() as Grid2D<BlockColors>);
        var tempGrid = (Grid2D<BlockColors>)grid.Clone();

        bool axisIsDiagonal = axis == new Vector2Int(1, -1) || axis == new Vector2Int(-1, 1) || axis == new Vector2Int(1, 1) || axis == new Vector2Int(-1, -1);

        foreach(var gridSquare in grid)
        {
            if(!gridSquare.value.Equals(BlockColors.Uninitialized))
            {
                if(axisIsDiagonal)
                {
                    HandleDiagonalMirror(position, axis, gridSquare, tempGrid);
                }
                else
                {
                    HandleStraightMirror(position, axis, gridSquare, tempGrid);
                }

            }

        }

        grid = tempGrid;

        foreach(Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        Pattern.SpawnPattern(grid, basePattern, grid.position, transform);

        SoundManager.Instance.Play(Audio.MultiplyPattern);

    }

    private void HandleStraightMirror(Vector2Int position, Vector2Int axis, Grid2D<BlockColors>.GridSpot<BlockColors> gridSquare, Grid2D<BlockColors> tempGrid)
    {
        int delta = Mathf.FloorToInt(Vector2.Dot(position - gridSquare.gridPosition, axis));
        Vector2Int mirrorPosition = gridSquare.gridPosition + axis * (2 * delta - ((axis.x > 0 || axis.y > 0) ? 1 : -1));

        if(grid.IsInsideGrid(mirrorPosition))
        {

            BlockColors newValue = BlockColors.Add(grid.GetValueAt(mirrorPosition), MirrorSquare(gridSquare.value, axis));

            tempGrid.SetValueAt(mirrorPosition, newValue);

        }
    }

    private void HandleDiagonalMirror(Vector2Int position, Vector2Int axis, Grid2D<BlockColors>.GridSpot<BlockColors> gridSquare, Grid2D<BlockColors> tempGrid)
    {
        int delta = FindClosestDistance(position, gridSquare.gridPosition, axis);
        for(int i = -100; i < 100; i++)
        {
            int potentialDelta = ManhattanDistance((position + i * new Vector2Int(axis.y, -axis.x)), gridSquare.gridPosition);
            delta = Mathf.Min(delta, potentialDelta);
        }

        Vector2Int mirrorPosition = gridSquare.gridPosition + axis * (delta + ((axis.x * axis.y > 0) ? (axis.x > 0 ? -1 : 1) : 0));
        if(delta == 1 && (axis.x * axis.y > 0) && axis.x < 0 && FindClosestDistance(position, gridSquare.gridPosition + new Vector2Int(2, 0), axis) == 1)
        {
            mirrorPosition = gridSquare.gridPosition;
        }


        if(grid.IsInsideGrid(mirrorPosition))
        {
            BlockColors newValue = gridSquare.value;
            if(mirrorPosition == gridSquare.gridPosition)
            {
                newValue = BlockColors.Add(grid.GetValueAt(mirrorPosition), MirrorSquareAxisThroughSquare(gridSquare.value, axis));
                tempGrid.SetValueAt(mirrorPosition, newValue);

            }
            else
            {
                float value = Vector2.Dot(mirrorPosition - position, axis);
                float value2 = Vector2.Dot(gridSquare.gridPosition - position, axis);
                if((value >= 0 && value2 < 0) || (value > 0 && value2 <= 0))
                {
                    newValue = BlockColors.Add(grid.GetValueAt(mirrorPosition), MirrorSquare(gridSquare.value, axis));
                    tempGrid.SetValueAt(mirrorPosition, newValue);
                }
            }
        }
    }

    private BlockColors MirrorSquare(BlockColors square, Vector2Int axis)
    {
        if(axis == new Vector2Int(0, 1) || axis == new Vector2Int(0, -1))
        {
            return new BlockColors(square.colors[0], square.colors[3], square.colors[2], square.colors[1]);


        }
        else if(axis == new Vector2Int(1, 0) || axis == new Vector2Int(-1, 0))
        {
            return new BlockColors(square.colors[2], square.colors[1], square.colors[0], square.colors[3]);

        }
        else if(axis == new Vector2Int(1, 1) || axis == new Vector2Int(-1, -1))
        {
            return new BlockColors(square.colors[3], square.colors[2], square.colors[1], square.colors[0]);
        }
        else if(axis == new Vector2Int(-1, 1) || axis == new Vector2Int(1, -1))
        {

            return new BlockColors(square.colors[1], square.colors[0], square.colors[3], square.colors[2]);
        }
        return square;
    }

    private BlockColors MirrorSquareAxisThroughSquare(BlockColors square, Vector2Int axis)
    {

        if(axis == new Vector2Int(1, 1))
        {
            return new BlockColors(square.colors[3], square.colors[2], square.colors[2], square.colors[3]);
        }
        if(axis == new Vector2Int(-1, -1))
        {
            return new BlockColors(square.colors[0], square.colors[1], square.colors[1], square.colors[0]);
        }
        if(axis == new Vector2Int(-1, 1))
        {
            return new BlockColors(square.colors[0], square.colors[0], square.colors[3], square.colors[3]);
        }
        if(axis == new Vector2Int(1, -1))
        {
            return new BlockColors(square.colors[1], square.colors[1], square.colors[2], square.colors[2]);
        }
        return square;
    }


    public void Update()
    {
        if(isFinishedPattern())
        {
            SoundManager.Instance.Play(Audio.Success);
            DrawingBridge.Instance?.EndDrawing(oldGrid.Count);
            if(SceneManager.GetSceneByName("PatternCreationRepair") != null)
            {
                //SceneManager.UnloadSceneAsync("PatternCreationRepair");
            }
        }

        showAxis();

        if(Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log(JsonUtility.ToJson(grid));
        }

        if(Input.GetKeyDown(KeyCode.O))
        {
            oldGrid = new Stack<Grid2D<BlockColors>>();
            grid = new Grid2D<BlockColors>(grid.gridSize, grid.position, grid.cellSize, BlockColors.Uninitialized);
            foreach(Transform child in transform)
            {
                Destroy(child.gameObject);
            }

            Pattern.SpawnPattern(grid, basePattern, grid.position, transform);

            SoundManager.Instance.Play(Audio.RemoveOrRevert);
        }

        if(Input.GetKeyDown(KeyCode.M))
        {
            if(oldGrid.Count > 0)
            {
                var backGrid = oldGrid.Pop();
                if(backGrid != null)
                {
                    grid = backGrid;
                    foreach(Transform child in transform)
                    {
                        Destroy(child.gameObject);
                    }

                    Pattern.SpawnPattern(grid, basePattern, grid.position, transform);
                }
            }

            SoundManager.Instance.Play(Audio.RemoveOrRevert);
        }

        DisableAllRenderers(!Input.GetKey(KeyCode.L));



    }

    public void showAxis()
    {

        Vector2Int axis = new Vector2Int((Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0), (Input.GetKey(KeyCode.W) ? 1 : 0) - (Input.GetKey(KeyCode.S) ? 1 : 0));

        if(axis != Vector2.zero)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 gridPosition = grid.GetWorldPosition(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)));

            ln.SetPosition(0, -1000 * new Vector3(axis.y, 0, -axis.x) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0, 50, 0));
            ln.SetPosition(1, (1000 - 1) * new Vector3(axis.y, 0, -axis.x) + new Vector3(gridPosition.x, 0, gridPosition.y) + new Vector3(0, 50, 0));

            arrow.transform.rotation = Quaternion.FromToRotation(Vector3.right, -new Vector3(axis.x, 0, axis.y));
            arrow.transform.position = new Vector3(gridPosition.x, 0, gridPosition.y);

            if(Input.GetMouseButtonDown(0))
            {
                MirrorPattern(grid.GetClosestGridPosition(new Vector2(mousePosition.x, mousePosition.z)), axis);
            }
        }
    }

    public bool isFinishedPattern()
    {
        if(wantedPattern == null)
        {
            return true;
        }
        return Enumerable.SequenceEqual(wantedPattern.pattern, grid);
    }

    public void DisableAllRenderers(bool state)
    {
        var allRenderers = gameObject.GetComponentsInChildren<Renderer>();
        foreach(Renderer childRenderer in allRenderers)
        {
            childRenderer.enabled = state;
        }
    }

    public static int ManhattanDistance(Vector2Int a, Vector2Int b)
    {
        checked
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }
    }

    public static int FindClosestDistance(Vector2Int a, Vector2Int b, Vector2Int axis)
    {
        int delta = 1000000;
        for(int i = -100; i < 100; i++)
        {
            int potentialDelta = ManhattanDistance((a + i * new Vector2Int(axis.y, -axis.x)), b);
            delta = Mathf.Min(delta, potentialDelta);
        }
        return delta;
    }

}
