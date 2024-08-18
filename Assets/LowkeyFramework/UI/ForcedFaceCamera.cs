using UnityEngine;

public class ForcedFaceCamera : MonoBehaviour
{
    // Update is called once per frame
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }
}
