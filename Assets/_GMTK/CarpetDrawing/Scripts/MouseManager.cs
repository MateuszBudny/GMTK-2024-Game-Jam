using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private Camera currentCamera;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            CameraDrag currentCameraDrag = currentCamera.GetComponent<CameraDrag>();
            Physics.Raycast(currentCameraDrag.dragCamera.ScreenToWorldPoint(Input.mousePosition), currentCameraDrag.transform.forward, out var raycastHit);
            Piece piece = raycastHit.collider?.transform.parent.GetComponent<Piece>();

            if(piece != null)
            {
                piece.StartDrag(raycastHit.point);
                scrollRect.StopMovement();
                scrollRect.enabled = false;

            }
            else
            {
                if(EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                currentCameraDrag.StartDrag();

            }


        }

        if(Input.GetMouseButtonUp(0))
        {
            scrollRect.enabled = true;
        }

    }
}
