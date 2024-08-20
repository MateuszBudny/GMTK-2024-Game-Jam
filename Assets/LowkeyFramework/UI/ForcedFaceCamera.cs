using UnityEngine;

public class ForcedFaceCamera : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        if(Camera.main != null)
        {
            transform.LookAt(Camera.main.transform);
        }
    }
}
