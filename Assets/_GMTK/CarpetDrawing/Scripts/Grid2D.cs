using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class Grid2D<T> : ICloneable, IEnumerable<Grid2D<T>.GridSpot<T>>
{
    
    [SerializeField] T[] values;
    [SerializeField] public Vector2Int gridSize;
    [SerializeField] public Vector2 position;
    [SerializeField] public Vector2 cellSize;

    T[] nonAllocReturnArray = new T[4];
    
    
    public Grid2D(Vector2Int gridSize, Vector2 position, Vector2 cellSize, T defaultValue = default)
    {
        this.gridSize = gridSize;
        this.position = position;
        this.cellSize = cellSize;

        values = Enumerable.Repeat(defaultValue,gridSize.x * gridSize.y).ToArray();
    }

    public Grid2D(Vector2Int gridSize, Vector2 position, Vector2 cellSize, T[] inputValues)
    {
        this.gridSize = gridSize;
        this.position = position;
        this.cellSize = cellSize;

        values = (T[])inputValues.Clone();
    }
    
    public int Length => values.Length;

    public T GetClosestValue(Vector2 pos)
    {
        Vector2 normalizedPos = GetGridPosition(pos);
        Vector2 clampedPos = new Vector2(Mathf.Clamp(normalizedPos.x, 0, gridSize.x - 1), Mathf.Clamp(normalizedPos.y, 0, gridSize.y - 1));
        Vector2Int index = new Vector2Int(Mathf.FloorToInt(clampedPos.x), Mathf.FloorToInt(clampedPos.y));
        return GetValueAt(index);
    }
    public T SetClosestValue(Vector2 pos, T value)
    {
        Vector2 normalizedPos = GetGridPosition(pos);
        Vector2 clampedPos = new Vector2(Mathf.Clamp(normalizedPos.x, 0, gridSize.x - 1), Mathf.Clamp(normalizedPos.y, 0, gridSize.y - 1));
        Vector2Int index = new Vector2Int(Mathf.FloorToInt(clampedPos.x), Mathf.FloorToInt(clampedPos.y));
        return SetValueAt(index, value);
    }

    public Vector2Int GetGridPosition(Vector2 pos)
    {
        return Vector2Int.FloorToInt(Vector2.Scale(pos - position, new Vector2(1 / cellSize.x, 1 / cellSize.y)));
    }
    public Vector2Int GetClosestGridPosition(Vector2 pos)
    {
        return Vector2Int.RoundToInt(Vector2.Scale(pos - position, new Vector2(1 / cellSize.x, 1 / cellSize.y)));
    }
    public Vector2 GetWorldPosition(Vector2Int pos)
    {
        return Vector2.Scale(pos, cellSize) + position;
    }

    T[] GetClosestValuesImpl(Vector2 pos, T[] array)
    {
        Vector2 normalizedPos = GetGridPosition(pos);
        Vector2 clampedPos = new Vector2(Mathf.Clamp(normalizedPos.x, 0, gridSize.x - 1), Mathf.Clamp(normalizedPos.y, 0, gridSize.y - 1));
        Vector2Int indexMinXMinY = new Vector2Int(Mathf.FloorToInt(clampedPos.x), Mathf.FloorToInt(clampedPos.y));
        Vector2Int indexMinXMaxY = new Vector2Int(Mathf.FloorToInt(clampedPos.x), Mathf.CeilToInt(clampedPos.y));
        Vector2Int indexMaxXMinY = new Vector2Int(Mathf.CeilToInt(clampedPos.x), Mathf.FloorToInt(clampedPos.y));
        Vector2Int indexMaxXMaxY = new Vector2Int(Mathf.CeilToInt(clampedPos.x), Mathf.CeilToInt(clampedPos.y));
        array[0] = GetValueAt(indexMinXMinY);
        array[1] = GetValueAt(indexMinXMaxY);
        array[2] = GetValueAt(indexMaxXMinY);
        array[3] = GetValueAt(indexMaxXMaxY);
        return array;
    }

    public T[] GetClosestValuesNonAlloc(Vector2 pos)
    {
        return GetClosestValuesImpl(pos, nonAllocReturnArray);
    }

    public T[] GetClosestValues(Vector2 pos)
    {
        return GetClosestValuesImpl(pos, new T[4]);
    }

    public int convertIndex(int x, int y)
    {
        return x + y * gridSize.x;
    }

    public int convertIndex(Vector2Int v)
    {
        return v.x  + v.y * gridSize.x;
    }

    public bool IsInsideGrid(Vector2 pos)
    {
        Vector2Int gridPosition = GetGridPosition(pos);
        return IsInsideGrid(gridPosition);
    }
    public bool IsInsideGrid(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0)
        {
            return false;
        }

        if (pos.x >= gridSize.x || pos.y >= gridSize.y)
        {
            return false;
        }

        return true;
    }

    public T GetValueAt(int x, int y)
    {
        return values[convertIndex(x,y)];
    }
    public T GetValueAt(Vector2Int v)
    {
        return values[convertIndex(v)];
    }
    public T GetValueAtClamp(Vector2Int v)
    {
        Vector2Int clampedPos = new Vector2Int(Mathf.Clamp(v.x, 0, gridSize.x - 1), Mathf.Clamp(v.y, 0, gridSize.y - 1));
        return values[convertIndex(clampedPos)];
    }
    public T SetValueAt(int x, int y, T value)
    {
        return values[convertIndex(x,y)] = value;
    }
    public T SetValueAt(Vector2Int v, T value)
    {
        return values[convertIndex(v)] = value;
    }

    public IEnumerator<GridSpot<T>> GetEnumerator()
    {
        for (int i = 0; i < gridSize.x; i++)
        {
            for (int j = 0; j < gridSize.y; j++)
            {
                yield return new GridSpot<T> { value = values[convertIndex(i,j)], gridPosition = new Vector2Int(i, j)};
            }
        }
    }

    public object Clone()
    {
        return new Grid2D<T>(gridSize, position, cellSize, values);
    }

    public struct GridSpot<U>
    {
        public U value;
        public Vector2Int gridPosition;
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}