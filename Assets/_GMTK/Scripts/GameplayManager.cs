using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : SingleBehaviour<GameplayManager>
{
    //[SerializeField]
    //private List<GameObject> turnOffOnOpeningCarpetDrawing;

    public void PrepareForEnteringCarpetDrawing()
    {
        //turnOffOnOpeningCarpetDrawing.ForEach(obj => obj.SetActive(false));
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        rootObjects.ForEach(root => root.SetActive(false));
    }

    public void PrepareForExitingCarpetDrawing()
    {
        //turnOffOnOpeningCarpetDrawing.ForEach(obj => obj.SetActive(true));

        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        rootObjects.ForEach(root => root.SetActive(true));
    }
}
