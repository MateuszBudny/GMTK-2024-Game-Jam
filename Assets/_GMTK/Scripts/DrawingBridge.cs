using System.Collections.Generic;
using UnityEngine;

public class DrawingBridge : SingleBehaviour<DrawingBridge>
{

    //Drawing carpet Inputs
    public Pattern startPattern;
    public Pattern wantedPattern;
    public List<Pattern> usablePatterns = new List<Pattern>();
    public int wallet;
    public CarpetRepairStation carpetRepairStation;

    public void EndDrawing(int carpetCost)
    {
        CursorManager.Instance.CurrentCursorLockMode = CursorLockMode.Locked;
        carpetRepairStation.Exit(carpetCost);
    }
}
