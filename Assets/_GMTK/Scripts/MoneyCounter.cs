using AetherEvents;
using TMPro;
using UnityEngine;

public class MoneyCounter : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI moneyTMP;

    private void Awake()
    {
        MoneyChanged.AddListener(OnMoneyChanged);
    }

    private void OnMoneyChanged(MoneyChanged context)
    {
        moneyTMP.text = $"Money: ${context.currentMoney}";
    }
}
