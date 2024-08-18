using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{

    [SerializeField] private CameraManager cameraManager;
    [SerializeField] private ScrollRect scrollRect;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            CameraDrag currentCamera = cameraManager.GetCamera(Input.mousePosition);
            Debug.Log(currentCamera.dragCamera.ScreenToWorldPoint(Input.mousePosition) + " " + currentCamera.transform.forward);
            Physics.Raycast(currentCamera.dragCamera.ScreenToWorldPoint(Input.mousePosition), currentCamera.transform.forward, out var raycastHit);
            Piece piece = raycastHit.collider?.transform.parent.GetComponent<Piece>();
            
            if (piece != null)
            {
                Debug.Log("Piece hit");
                piece.StartDrag(raycastHit.point);
                scrollRect.StopMovement();
                scrollRect.enabled = false;
                
            }
            else
            {
                if (EventSystem.current.IsPointerOverGameObject())
                {
                    return;
                }

                currentCamera.StartDrag();

            }
            
            
        }

        if (Input.GetMouseButtonUp(0))
        {
            scrollRect.enabled = true;
        }
        
    }
}
