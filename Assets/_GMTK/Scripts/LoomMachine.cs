using DG.Tweening;
using UnityEngine;

public class LoomMachine : MonoBehaviour
{
    [SerializeField]
    private InteractionReceiver interactionReceiver;
    [SerializeField]
    private Transform visualsParent;

    [Header("Not Bought State")]
    [SerializeField]
    private int priceToBuy = 10;
    [SerializeField]
    private GameObject notBoughtVisual;

    [Header("Bought State")]
    [SerializeField]
    private GameObject boughtVisual;
    [SerializeField]
    private string boughtStateInteractionInfo = "[E] To Weave";

    private Tween cantAffordTween;

    public LoomState State
    {
        get => state;
        set
        {
            PreviousState = state;
            state = value;

            switch(state)
            {
                case LoomState.FreshlyBought:
                    SetVisualsToBought();
                    State = LoomState.Idle;
                    break;

            }
        }
    }

    public LoomState PreviousState { get; private set; }

    private LoomState state = LoomState.ToBeBought;

    public void OnInteract()
    {
        switch(State)
        {
            case LoomState.ToBeBought:
                if(PlayerWallet.Instance.TryToBuy(priceToBuy))
                {
                    State = LoomState.FreshlyBought;
                }
                else
                {
                    if(cantAffordTween != null)
                    {
                        cantAffordTween.Kill(true);
                    }
                    cantAffordTween = visualsParent.DOPunchPosition(Vector3.right / 3f, 1f, 10, 0.3f);
                }
                break;
            case LoomState.Idle:
                break;
            case LoomState.Looming:
                break;
        }
    }

    private void SetVisualsToBought()
    {
        notBoughtVisual.SetActive(false);
        boughtVisual.SetActive(true);

        interactionReceiver.InteractionInfo = boughtStateInteractionInfo;
    }

    public enum LoomState
    {
        ToBeBought,
        FreshlyBought,
        Idle,
        Looming,
    }
}
