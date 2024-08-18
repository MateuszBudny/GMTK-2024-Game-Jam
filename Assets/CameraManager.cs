using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraManager : MonoBehaviour
{

    [SerializeField] private CameraDrag worldCamera;
    [SerializeField] private CameraDrag inventoryCamera;
    [SerializeField] private CameraDrag moveCamera;

    [SerializeField] private float split = 0.7f;

    [SerializeField] private float activationPercent = 0.9f;

    [SerializeField] private int framesToShow = 10;

    private bool isInventoryVisible = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.mousePosition.x > Screen.width * activationPercent)
        {
            isInventoryVisible = true;
            //StopCoroutine(hideInventory());
            //StartCoroutine(showInventory());

        }

        if (isInventoryVisible && Input.mousePosition.x < Screen.width * split)
        {
            //StopCoroutine(showInventory());
            //StartCoroutine(hideInventory());
        }
    }
    
    public IEnumerator showInventory()
    {
        for (int i = 0; i < framesToShow; i++)
        {
            
            isInventoryVisible = true;
            Rect worldRect = worldCamera.GetComponent<Camera>().rect;
            worldRect.xMax = Mathf.Lerp(1,split,(float)i/framesToShow);
            worldCamera.dragCamera.rect = worldRect;
                
            Rect invRect = inventoryCamera.dragCamera.rect;
            invRect.x = Mathf.Lerp(1,split,(float)i/framesToShow);
            inventoryCamera.dragCamera.rect = invRect;
            
            yield return false;
        }
        
    }
    
    public IEnumerator hideInventory()
    {
        for (int i = 0; i < framesToShow; i++)
        {
            isInventoryVisible = true;
            Rect worldRect = worldCamera.dragCamera.rect;
            worldRect.xMax = Mathf.Lerp(split,1,(float)i/framesToShow);
            worldCamera.dragCamera.rect = worldRect;
            
            Rect invRect = inventoryCamera.dragCamera.rect;
            invRect.x = Mathf.Lerp(split,1,(float)i/framesToShow);
            inventoryCamera.dragCamera.rect = invRect;
            
            yield return false;
        }
        
    }

    public CameraDrag GetCamera(Vector3 mousePosition)
    {
        if (worldCamera.dragCamera.pixelRect.Contains(mousePosition))
        {
            return worldCamera;
        }

        if (inventoryCamera.dragCamera.pixelRect.Contains(mousePosition))
        {
            return inventoryCamera;
        }

        return null;
    }
}
