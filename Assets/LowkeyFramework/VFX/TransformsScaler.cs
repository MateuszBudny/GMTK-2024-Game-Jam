using System.Collections.Generic;
using UnityEngine;

public class TransformsScaler : MonoBehaviour
{
    [SerializeField]
    private List<Transform> transformsToScale;

    public void SetTransformsScale(float scale)
    {
        transformsToScale.ForEach(t => t.localScale = new Vector3(scale, scale, scale));
    }
}
