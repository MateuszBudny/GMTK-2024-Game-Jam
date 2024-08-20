using UnityEngine;
using UnityEngine.SceneManagement;

public class CarpetRepairStation : MonoBehaviour
{
    [SerializeField]
    private TakeableItemType carpetTakeableItemType;
    [SerializeField]
    private InteractionReceiver interactionReceiver;
    [TextArea]
    [SerializeField]
    private string isPossibleToPutCarpetInteractionInfoText;
    [TextArea]
    [SerializeField]
    private string isNotPossibleToPutCarpetInteractionInfoText;
    [SerializeField]
    private Transform carpetSlot;

    [SerializeField]
    private Carpet carpetInside;

    public bool CanCarpetBePutInside => PhysicalInventory.Instance.HasItemOfType(carpetTakeableItemType) && !carpetInside;

    private void Update()
    {
        if(CanCarpetBePutInside)
        {
            interactionReceiver.InteractionInfo = isPossibleToPutCarpetInteractionInfoText;
        }
        else
        {
            interactionReceiver.InteractionInfo = isNotPossibleToPutCarpetInteractionInfoText;
        }
    }

    public void Enter()
    {
        if(!carpetInside)
        {
            return;
        }

        DrawingBridge.Instance.wallet = PlayerWallet.Instance.Money;
        DrawingBridge.Instance.startPattern = carpetInside.startingPattern;
        DrawingBridge.Instance.wantedPattern = carpetInside.wantedPattern;
        DrawingBridge.Instance.usablePatterns = carpetInside.usablePatterns;
        DrawingBridge.Instance.carpetRepairStation = this;

        GameplayManager.Instance.PrepareForEnteringCarpetDrawing();

        SceneManager.LoadScene("PatternCreationRepair", LoadSceneMode.Additive);

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach(GameObject a in allObjects)
        {
            if(a.name == "EventSystem")
            {
                continue;
            }

            a.SetActive(false);
        }
    }

    public void Exit(int carpetCost)
    {


        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Małpkarnia"));

        GameObject[] allObjects = FindObjectsOfType<GameObject>(true);

        foreach(GameObject a in allObjects)
        {
            a.SetActive(true);
        }

        GameplayManager.Instance.PrepareForExitingCarpetDrawing();

        PlayerWallet.Instance.Money -= carpetCost;
        Debug.Log("exit");
    }

    public void TryToPutCarpet()
    {
        if(!CanCarpetBePutInside)
        {
            return;
        }

        carpetInside = PhysicalInventory.Instance.TryToTakeItemOfType(carpetTakeableItemType).GetComponent<Carpet>();
        carpetInside.transform.parent = carpetSlot;
        carpetInside.transform.SetPositionAndRotation(carpetSlot.position, carpetSlot.rotation);
        carpetInside.GetComponent<Rigidbody>().isKinematic = true;
        carpetInside.CarpetPutIntoRepairStation(this);
    }

    public void CarpetRemoved()
    {
        carpetInside = null;
    }
}
