using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{

    [SerializeField] Pattern pattern;
    [SerializeField] PatternGrid patternGrid;


    private Collider2D roughCollider;
    private Dictionary<Vector2Int, GameObject> pieceObjects = new Dictionary<Vector2Int, GameObject>();

    private Transform oldParent;

    void Start()
    {
        if(pattern == null)
        {
            Debug.LogError("Piece must have a pattern");
            return;
        }


        pieceObjects = Pattern.SpawnPattern(pattern.pattern, pattern.singleBlock, transform.position, transform);

        RectTransform parentRect = transform.parent?.GetComponent<RectTransform>();
        if(parentRect != null)
        {
            parentRect.sizeDelta = new Vector2(pattern.size.x, pattern.size.y) * 200;

            Vector3[] corners = new Vector3[4];
            parentRect.GetWorldCorners(corners);
            transform.position = corners[0];
        }

        roughCollider = GetComponent<Collider2D>();

    }


    public void StartDrag(Vector3 position)
    {
        if(transform.parent != null)
        {
            Piece piece = Instantiate(gameObject, transform.parent).GetComponent<Piece>();
            foreach(Transform child in piece.transform)
            {
                Destroy(child.gameObject);
            }

            piece.transform.parent = null;
            piece.transform.localScale *= pattern.singleBlock.transform.localScale.x / piece.transform.lossyScale.x;
            StartCoroutine(piece.FollowMouse((position - transform.position) * 0.14f / piece.transform.localScale.x));
        }
        else
        {
            StartCoroutine(FollowMouse(position - transform.position));
        }
    }


    public IEnumerator FollowMouse(Vector3 delta)
    {
        float currentY = 20;
        while(true)
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - delta;
            newPosition.y = currentY;
            transform.position = newPosition;
            if(Input.GetMouseButtonUp(0))
            {
                patternGrid.AddPattern(transform.position, pattern);
                Destroy(gameObject);
                break;
            }
            yield return null;
        }
    }

    public void Update()
    {

    }
}
