using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Pattern", menuName = "ScriptableObjects/Pattern", order = 1)]
public class Pattern : ScriptableObject
{
    [SerializeField] public GameObject singleBlock;
    [SerializeField] public Vector2Int size;

    [HideInInspector]
    private List<BlockColors> pattern = new List<BlockColors>();

    public BlockColors GetAt(int x, int y)
    {
        return pattern[x + y * size.x];
    }
    
    
    public BlockColors SetAt(int x, int y, BlockColors color)
    {
        return (pattern[x + y * size.x] = color);
    }

    public void Resize(int x, int y)
    {
        if (pattern.Count == x * y)
        {
            return;
        }
        pattern = Enumerable.Repeat(BlockColors.Uninitialized, x * y).ToList();
    }
    
}
