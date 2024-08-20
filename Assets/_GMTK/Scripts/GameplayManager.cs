using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : SingleBehaviour<GameplayManager>
{
    [SerializeField]
    private List<GameObject> turnOffOnOpeningCarpetDrawing;

    public void PrepareForEnteringCarpetDrawing()
    {
        turnOffOnOpeningCarpetDrawing.ForEach(obj => obj.SetActive(false));
    }

    public void PrepareForExitingCarpetDrawing()
    {
        turnOffOnOpeningCarpetDrawing.ForEach(obj => obj.SetActive(true));
    }
}
