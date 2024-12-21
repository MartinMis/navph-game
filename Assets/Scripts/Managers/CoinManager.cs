using System;
using UnityEngine;

namespace Managers
{
    /// <summary>
    /// Singleton class for managing coins
    /// </summary>
    public class CoinManager : MonoBehaviour, ICoinManager
    {
        // Singleton instance
        [Tooltip("Coins when first starting the game")]
        [SerializeField] private int initialCoins;
       
        public static ICoinManager Instance { get; private set; }
        public int RunEarnings { get; private set; } = 0;  // in game run earnings

        int _totalCoins; // global total coins 
        
        /// <summary>
        /// Invoke when total coins change
        /// </summary>
        public event Action OnCoinsChanged;
        
        /// <summary>
        /// Invoke when run coins change
        /// </summary>
        public event Action OnRunEarningsChanged;
        
        private const string TotalCoinsKey = "TotalCoins"; // for PlayerPrefs

        /// <summary>
        /// Initializer for the singleton
        /// </summary>
        public void Initialize()
        {
            Debug.Log("[CoinManager] Initializing...");
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            LoadCoins();
        }

        /// <summary>
        /// Getter for total number of coins
        /// </summary>
        /// <returns>Total number of coins</returns>
        public int GetTotalCoins()
        {
            return _totalCoins;
        }
        
        /// <summary>
        /// When added or spent coins
        /// </summary>
        private void SaveAndInvoke() 
        { 
            SaveCoins();
            OnCoinsChanged?.Invoke();
        }
        
        /// <summary>
        /// Add coinst to players total
        /// </summary>
        /// <remarks>
        /// future feature; can be used in events or achievements
        /// or when player buys coins for real money $$$
        /// </remarks>
        public void AddCoins(int amount)
        {
            _totalCoins += amount;
            Debug.Log($"Added {amount} coins. Total: {_totalCoins}");
            SaveAndInvoke();
        }


        /// <summary>
        /// When player buys something in the game
        /// </summary>
        public bool SpendCoins(int amount)
        {
            if (_totalCoins >= amount)
            {
                _totalCoins -= amount;
                Debug.Log($"Spent {amount} coins. Remaining: {_totalCoins}");
                SaveAndInvoke();
                return true;
            }
            Debug.Log("Not enough coins!");
            return false;
        }
        
        /// <summary>
        /// Will be called during the game run each time player collects coins
        /// </summary>
        public void AddRunEarnings(int amount)
        {
            RunEarnings += amount;
            OnRunEarningsChanged?.Invoke();
            Debug.Log($"Run earnings: {RunEarnings}");
        }
        
        /// <summary>
        /// When player finishes the run, add the run earnings to the total
        /// </summary>
        public void FinalizeRunEarnings()
        {
            _totalCoins += RunEarnings;
            Debug.Log($"Run earnings {RunEarnings} added to total. New total: {_totalCoins}");
            RunEarnings = 0; // Reset earnings
            SaveCoins(); // Ulož nový stav
            OnCoinsChanged?.Invoke();
        }
        
        /// <summary>
        /// Saves the total coins to PlayerPrefs.
        /// </summary>
        private void SaveCoins()
        {
            PlayerPrefs.SetInt(TotalCoinsKey, _totalCoins);
            PlayerPrefs.Save(); // Zapíš dáta na disk
            Debug.Log($"[CoinManager] Coins saved: {_totalCoins}");
        }

        /// <summary>
        /// Loads the total coins from PlayerPrefs.
        /// </summary>
        private void LoadCoins()
        {
            if (PlayerPrefs.HasKey(TotalCoinsKey))
            {
                _totalCoins = PlayerPrefs.GetInt(TotalCoinsKey);
                Debug.Log($"[CoinManager] Loaded coins: {_totalCoins}");
            }
            else
            {
                _totalCoins = initialCoins; // Predvolený poèet coinov
                Debug.Log($"[CoinManager] No saved coins found. Defaulting to {_totalCoins}");
            }
        }
        
        /// <summary>
        /// Method for resetting the coins
        /// </summary>
        public void ResetCoins()
        {
            LoadCoins();
            OnCoinsChanged?.Invoke();
        }
    }
}
