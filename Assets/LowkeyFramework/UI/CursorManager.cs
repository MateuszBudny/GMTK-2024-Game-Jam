using UnityEngine;

public class CursorManager : SingleBehaviour<CursorManager>
{
    [SerializeField]
    private CursorLockMode startingCursorLockMode = CursorLockMode.Locked;

    public CursorLockMode CurrentCursorLockMode
    {
        get => currentCursorLockMode;
        set
        {
            currentCursorLockMode = value;
            Cursor.lockState = value;
        }
    }

    private CursorLockMode currentCursorLockMode = CursorLockMode.Locked;

    private void Start()
    {
        CurrentCursorLockMode = startingCursorLockMode;
    }

    public void OnApplicationFocus(bool hasFocus)
    {
        if(hasFocus)
        {
            Cursor.lockState = CurrentCursorLockMode;
        }
    }
}
