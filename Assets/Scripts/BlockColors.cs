using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ParadoxNotion.Design;
using UnityEngine;

[Serializable]
public struct BlockColors : IEquatable<BlockColors>
{
    public LeafColor[] colors;

    public static BlockColors Uninitialized => new BlockColors(LeafColor.Uninitialized);
    
    public BlockColors(LeafColor[] c)
    {
        colors = c;
    }

    public BlockColors(LeafColor c1, LeafColor c2, LeafColor c3, LeafColor c4)
    {
        colors = new LeafColor[] { c1, c2, c3, c4 };
    }
    public BlockColors(LeafColor c1)
    {
        colors = new LeafColor[] { c1, c1, c1, c1 };
    }
    
    public static Color LeafColorToColor(LeafColor leaf)
    {
        Dictionary<LeafColor, Color> colors = new Dictionary<LeafColor, Color>
        {
            { LeafColor.Black, Color.black },
            { LeafColor.White, Color.white },
            { LeafColor.Red, Color.red },
            { LeafColor.Blue, Color.blue },
            { LeafColor.Uninitialized, new Color(0,0,0,0) }
        };
        return colors[leaf];
    }

    public bool Equals(BlockColors other)
    {
        return Enumerable.SequenceEqual(colors, other.colors);
    }

    public override bool Equals(object obj)
    {
        return obj is BlockColors other && Equals(other);
    }

    public override int GetHashCode()
    {
        return (colors != null ? colors.GetHashCode() : 0);
    }

    public static BlockColors Add(BlockColors lhs, BlockColors rhs)
    {
        return new BlockColors(rhs.colors[0] == LeafColor.Uninitialized ? lhs.colors[0] : rhs.colors[0], 
            rhs.colors[1] == LeafColor.Uninitialized ? lhs.colors[1] : rhs.colors[1],
            rhs.colors[2] == LeafColor.Uninitialized ? lhs.colors[2] : rhs.colors[2],
        rhs.colors[3] == LeafColor.Uninitialized ? lhs.colors[3] : rhs.colors[3]);
    }
}


public enum LeafColor 
{
    Uninitialized,
    White,
    Black,
    Red,
    Blue,
}



