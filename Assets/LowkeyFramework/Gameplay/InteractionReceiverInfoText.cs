using TMPro;
using UnityEngine;

public class InteractionReceiverInfoText : MonoBehaviour
{
    [SerializeField]
    private InteractionReceiver receiver;
    [SerializeField]
    private TextMeshPro infoTMP;

    private void Awake()
    {
        OnStoppedBeingTheCurrentInteraction();
    }

    public void OnStartedBeingTheCurrentInteraction()
    {
        infoTMP.text = receiver.InteractionInfo;
        infoTMP.gameObject.SetActive(true);
    }

    public void OnStoppedBeingTheCurrentInteraction()
    {
        infoTMP.gameObject.SetActive(false);
    }
}
