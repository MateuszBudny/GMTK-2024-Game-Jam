using AetherEvents;
using UnityEngine;

public class PlayerWallet : SingleBehaviour<PlayerWallet>
{
    [SerializeField]
    private int money = 10;

    public int Money
    {
        get => money;
        set
        {
            if(value < 0)
            {
                money = 0;
            }
            else
            {
                money = value;
            }

            new MoneyChanged(Money).Invoke();
        }
    }

    private void Start()
    {
        new MoneyChanged(Money).Invoke();
    }

    public bool CanAfford(int cost) => Money >= cost;

    public bool TryToBuy(int cost)
    {
        if(!CanAfford(cost))
            return false;

        Money -= cost;
        return true;
    }
}
