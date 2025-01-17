using DG.Tweening;
using TMPro;
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
    [TextArea]
    [SerializeField]
    private string carpetNeedsToBeTakenOutInteractionInfoText;
    [SerializeField]
    private Transform carpetSlot;
    [SerializeField]
    private Transform visualToShake;
    [SerializeField]
    private TextMeshPro repairInfoTMP;

    [SerializeField]
    private DialogueSequenceSO firstCarpetRepairedDialogue;
    [SerializeField]
    private DialogueSequenceSO secondRepairDialogue;

    public Carpet CarpetInside { get; private set; }

    public bool CanCarpetBePutInside => PhysicalInventory.Instance.HasItemOfType(carpetTakeableItemType) && !CarpetInside;
    public bool CanCarpetBeTakenOut => CarpetInside && PhysicalInventory.Instance.HasFreeSpace;

    private Tween shakeTween;
    private int whichExitIsIt = 0;

    private void Awake()
    {
        repairInfoTMP.text = "";
    }

    private void Update()
    {
        if(CanCarpetBePutInside)
        {
            interactionReceiver.InteractionInfo = isPossibleToPutCarpetInteractionInfoText;
        }
        else if(CanCarpetBeTakenOut)
        {
            interactionReceiver.InteractionInfo = $"{carpetNeedsToBeTakenOutInteractionInfoText}\n{isNotPossibleToPutCarpetInteractionInfoText}";
        }
        else
        {
            interactionReceiver.InteractionInfo = isNotPossibleToPutCarpetInteractionInfoText;
        }
    }

    public void Enter()
    {
        if(!CarpetInside || CarpetInside.IsFinished)
        {
            if(shakeTween != null)
            {
                shakeTween.Kill(true);
            }
            shakeTween = visualToShake.DOPunchPosition(Vector3.right / 5f, 1f, 8, 0.1f);
            return;
        }

        DrawingBridge.Instance.wallet = PlayerWallet.Instance.Money;
        DrawingBridge.Instance.startPattern = CarpetInside.startingPattern;
        DrawingBridge.Instance.wantedPattern = CarpetInside.wantedPattern;
        DrawingBridge.Instance.usablePatterns = CarpetInside.usablePatterns;
        DrawingBridge.Instance.carpetRepairStation = this;

        GameplayManager.Instance.PrepareForEnteringCarpetDrawing();

        SceneManager.LoadScene("PatternCreationRepair", LoadSceneMode.Additive);

        SoundManager.Instance.Play(Audio.EnterStation);
    }

    public void Exit(int carpetCost)
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Małpkarnia"));

        GameplayManager.Instance.PrepareForExitingCarpetDrawing();

        PlayerWallet.Instance.Money -= carpetCost;
        CarpetInside.IsFinished = true;

        repairInfoTMP.text = $"-${carpetCost}";
        Invoke(nameof(ResetRepairInfo), 5f);

        whichExitIsIt++;
        if(whichExitIsIt == 1)
        {
            firstCarpetRepairedDialogue.StartDialogue();
        }
        else if(whichExitIsIt == 2)
        {
            secondRepairDialogue.StartDialogue();
        }

        Debug.Log("exit");
    }

    private void ResetRepairInfo()
    {
        repairInfoTMP.text = "";
    }

    public void TryToDealWithCarpet()
    {
        if(CarpetInside)
        {
            TryToTakeCarpet();
        }
        else
        {
            TryToPutCarpet();
        }
    }

    public void TryToPutCarpet()
    {
        if(!CanCarpetBePutInside)
            return;

        CarpetInside = PhysicalInventory.Instance.TryToTakeItemOfType(carpetTakeableItemType).GetComponent<Carpet>();
        CarpetInside.transform.parent = carpetSlot;
        CarpetInside.transform.SetPositionAndRotation(carpetSlot.position, carpetSlot.rotation);
        CarpetInside.GetComponent<Rigidbody>().isKinematic = true;
        CarpetInside.CarpetPutIntoRepairStation(this);

        SoundManager.Instance.Play(Audio.PutDownOnProperPlace);
    }

    public void TryToTakeCarpet()
    {
        if(!CanCarpetBeTakenOut)
            return;

        CarpetInside.TryToTakeItem();
    }

    public void CarpetRemoved()
    {
        CarpetInside = null;
    }
}
