using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlantsVsZombies.GrowSystem
{
    public interface IPvZGameCurrency
    {
        int CurrentMoney { get; }
    }

    public class PvZHardCurrency : IPvZGameCurrency
    {
        public PvZHardCurrency(int money)
        {
            this.CurrentMoney = money;
        }

        public int CurrentMoney
        {
            get;
            private set;
        }
    }
}
