using System;

namespace Managers
{
    /// <summary>
    /// Interface describing the coin manager
    /// </summary>
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
