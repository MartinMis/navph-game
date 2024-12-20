using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public interface ICoinManager

    {
        event Action OnCoinsChanged;
        public event Action OnRunEarningsChanged;
        int GetTotalCoins();    
        void AddCoins(int amount);
        bool SpendCoins(int amount); 
        void AddRunEarnings(int amount);
        void FinalizeRunEarnings();
        void ResetCoins();
        public int RunEarnings {get;}
    }
}
