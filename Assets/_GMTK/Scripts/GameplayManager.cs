using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayManager : SingleBehaviour<GameplayManager>
{
    //[SerializeField]
    //private List<GameObject> turnOffOnOpeningCarpetDrawing;

    [SerializeField]
    private DialogueSequenceSO prologueDialogue;
    [SerializeField]
    private BlackScreen blackScreen;

    private void Start()
    {
        prologueDialogue.StartDialogue();
        blackScreen.FadeOut(5f);
    }

    public void PrepareForEnteringCarpetDrawing()
    {
        //turnOffOnOpeningCarpetDrawing.ForEach(obj => obj.SetActive(false));
        List<GameObject> rootObjects = new List<GameObject>();
        Scene scene = SceneManager.GetActiveScene();
        scene.GetRootGameObjects(rootObjects);

        rootObjects.Where(root => root.name != "GameplayManager").ToList().ForEach(root => root.SetActive(false));
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
