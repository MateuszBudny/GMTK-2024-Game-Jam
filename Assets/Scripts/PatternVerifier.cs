/*
using System.Collections;
using System.Linq;
using UnityEngine;

public class PatternVerifier : MonoBehaviour
{

    [SerializeField] public Vector2Int gridSize = new Vector2Int(100,100);
    [SerializeField] public Vector2Int blockSize = new Vector2Int(5,5);

    [SerializeField] public GameObject block;


    public Vector2Int start;
    public Vector2Int offset;
    public Vector2Int secondOffset;
    public Pattern pattern;
    public Pattern patternToVerify;
    public BlockColors[,] grid;
    public BlockColors[,] tempGrid;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        grid = new BlockColors[gridSize.x, gridSize.y];
        tempGrid = new BlockColors[gridSize.x, gridSize.y];
        for (int i = 0; i < gridSize.x; i++)
        {
            for(int j = 0; j < gridSize.y; j++)
            {
                GameObject CurrentBlock = Instantiate(block, new Vector3(block.transform.localScale.x * i, -1, block.transform.localScale.x * j), block.transform.rotation, transform);
                CurrentBlock.GetComponent<SpriteRenderer>().color = new Color(1,1,1,0.5f);
                grid[i, j] = pattern.GetAt(i%blockSize.x,j%blockSize.y);
                tempGrid[i, j] = BlockColors.Uninitialized;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine( verifyGrid(start, offset, secondOffset, patternToVerify));
        }
    }

    public IEnumerator verifyGrid(Vector2Int startPosition, Vector2Int offset, Vector2Int secondOffset, Pattern chosenPattern)
    {

        
        int xSteps = Mathf.CeilToInt((gridSize.x + blockSize.x)) * 2;
        int ySteps = Mathf.CeilToInt((gridSize.y + blockSize.y)) * 2;

        for(int x = -xSteps; x < xSteps; x++)
        {
            for(int y = -ySteps; y < ySteps; y++)
            {
                float waitTime = 0f;
                Color randomColor = Random.ColorHSV(0,0.3f);
                for (int xPattern = 0; xPattern < blockSize.x; xPattern++)
                {
                    for (int yPattern = 0; yPattern < blockSize.y; yPattern++)
                    {
                        Vector2Int currentPosition = x * offset + y * secondOffset + new Vector2Int(xPattern,yPattern);
                        if (currentPosition.x >= 0 && currentPosition.y >= 0 && currentPosition.x < gridSize.x && currentPosition.y < gridSize.y)
                        {
                            if ((grid[currentPosition.x, currentPosition.y] != chosenPattern.GetAt(xPattern, yPattern) && chosenPattern.GetAt(xPattern, yPattern) != BlockColors.Uninitialized) || tempGrid[currentPosition.x, currentPosition.y] != BlockColors.Uninitialized)
                            {
                                Debug.Log("Patten failed");
                            }
                            if (chosenPattern.GetAt(xPattern, yPattern) != BlockColors.Uninitialized)
                            {
                                waitTime = 0.1f;
                                Instantiate(block, new Vector3(block.transform.localScale.x*currentPosition.x,0, block.transform.localScale.x * currentPosition.y), block.transform.rotation, transform).GetComponent<SpriteRenderer>().color = randomColor;
                                tempGrid[currentPosition.x, currentPosition.y] = chosenPattern.GetAt(xPattern, yPattern);
                            }
                        }
                    }
                    
                }
                if(waitTime > 0)
                    yield return new WaitForSeconds(waitTime);
            }

        }

        if(tempGrid.Cast<BlockColors>().Any(x => x == BlockColors.Uninitialized))
        {
            Debug.Log("Patten failed");
        }
        else
        {
            Debug.Log("Pattern fits");
        }
    }
    
}
*/