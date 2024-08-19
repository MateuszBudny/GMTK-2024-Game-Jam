using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetSquareRenderer : MonoBehaviour
{
    [SerializeField]
    List<Transform> leafs = new List<Transform>();

    public void SetColors(BlockColors block, float alpha = 1)
    {
        for(int i = 0; i < 4; i++)
        {
            Color color = BlockColors.LeafColorToColor(block.colors[i]);
            color.a *= alpha;
            leafs[i].GetComponent<SpriteRenderer>().color = color;    
        }
    }
}
