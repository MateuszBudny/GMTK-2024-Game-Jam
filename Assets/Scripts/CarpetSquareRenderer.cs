using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarpetSquareRenderer : MonoBehaviour
{
    [SerializeField]
    List<Transform> leafs = new List<Transform>();

    public void SetColors(BlockColors block)
    {
        for(int i = 0; i < 4; i++){
            leafs[i].GetComponent<SpriteRenderer>().color = BlockColors.LeafColorToColor(block.colors[i]);    
        }
    }
}
