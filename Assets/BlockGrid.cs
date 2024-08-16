using UnityEngine;

public class BlockGrid : MonoBehaviour
{

    [SerializeField] public Vector2Int gridSize = new Vector2Int(100,100);
    [SerializeField] public Vector2Int blockSize = new Vector2Int(5,5);
        
    public Colors[,] pattern;
    public Colors[,] grid;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grid = new Colors[gridSize.x, gridSize.y];
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public enum Colors
    {
        White,
        Black
    }
    
}
