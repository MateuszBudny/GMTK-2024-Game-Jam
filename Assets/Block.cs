using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    BlockColors[,] blocks;

    [SerializeField] private GameObject singleBlock;
    void Start()
    {
        for (int i = 0; i < blocks.GetLength(0); i++)
        {
            for(int j = 0; j < blocks.GetLength(1); j++)
            {
                Instantiate(singleBlock, new Vector3(i, 0, j) * singleBlock.transform.localScale.x, singleBlock.transform.rotation, transform);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
