using TMPro;
using UnityEngine;

public class GameVersionText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI versionTMP;

    private void Awake()
    {
        versionTMP.text = $"v{Application.version}";
    }
}
