using Aether;

namespace AetherEvents
{
    public class MoneyChanged : Event<MoneyChanged>
    {
        public readonly int currentMoney;

        public MoneyChanged(int currentMoney)
        {
            this.currentMoney = currentMoney;
        }
    }
}