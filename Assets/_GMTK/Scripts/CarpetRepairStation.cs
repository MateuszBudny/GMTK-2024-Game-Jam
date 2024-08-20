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

    public Carpet CarpetInside { get; private set; }

    public bool CanCarpetBePutInside => PhysicalInventory.Instance.HasItemOfType(carpetTakeableItemType) && !CarpetInside;

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
        if(!CarpetInside)
        {
            return;
        }

        DrawingBridge.Instance.wallet = PlayerWallet.Instance.Money;
        DrawingBridge.Instance.startPattern = CarpetInside.startingPattern;
        DrawingBridge.Instance.wantedPattern = CarpetInside.wantedPattern;
        DrawingBridge.Instance.usablePatterns = CarpetInside.usablePatterns;
        DrawingBridge.Instance.carpetRepairStation = this;

        GameplayManager.Instance.PrepareForEnteringCarpetDrawing();

        SceneManager.LoadScene("PatternCreationRepair", LoadSceneMode.Additive);
    }

    public void Exit(int carpetCost)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Małpkarnia"));

        GameplayManager.Instance.PrepareForExitingCarpetDrawing();

        PlayerWallet.Instance.Money -= carpetCost;
        CarpetInside.IsFinished = true;
        Debug.Log("exit");
    }

    public void TryToPutCarpet()
    {
        if(!CanCarpetBePutInside)
        {
            return;
        }

        CarpetInside = PhysicalInventory.Instance.TryToTakeItemOfType(carpetTakeableItemType).GetComponent<Carpet>();
        CarpetInside.transform.parent = carpetSlot;
        CarpetInside.transform.SetPositionAndRotation(carpetSlot.position, carpetSlot.rotation);
        CarpetInside.GetComponent<Rigidbody>().isKinematic = true;
        CarpetInside.CarpetPutIntoRepairStation(this);
    }

    public void CarpetRemoved()
    {
        CarpetInside = null;
    }
}
