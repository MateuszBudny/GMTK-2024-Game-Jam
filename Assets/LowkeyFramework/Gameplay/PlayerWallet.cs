using System;
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
                throw new ArgumentOutOfRangeException("You can't have negative money!");
            }

            money = value;
        }
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
