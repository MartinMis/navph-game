using System;
using UnityEngine;

namespace Managers
{
    public class CoinManager : MonoBehaviour, ICoinManager
    {
        // Singleton instance
        [SerializeField] private int initialCoins;
        public static ICoinManager Instance { get; private set; }

        int totalCoins; // global total coins 
        public int RunEarnings { get; private set; } = 0;  // in game run earnings

        public event Action OnCoinsChanged;
        public event Action OnRunEarningsChanged;

        private const string TotalCoinsKey = "TotalCoins"; // for PlayerPrefs

        public void Initialize()
        {
            Debug.Log("[CoinManager] Initializing...");
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            LoadCoins();
        }


        public int GetTotalCoins()
        {
            return totalCoins;
        }

        // When added or spent coins 
        private void SaveAndInvoke() 
        { 
            SaveCoins();
            OnCoinsChanged?.Invoke();
        }

        // future feature; can be used in events or achievements
        // or when player buys coins for real money $$$
        public void AddCoins(int amount)
        {
            totalCoins += amount;
            Debug.Log($"Added {amount} coins. Total: {totalCoins}");
            SaveAndInvoke();
        }



        // when player buys something in the game
        public bool SpendCoins(int amount)
        {
            if (totalCoins >= amount)
            {
                totalCoins -= amount;
                Debug.Log($"Spent {amount} coins. Remaining: {totalCoins}");
                SaveAndInvoke();
                return true;
            }
            Debug.Log("Not enough coins!");
            return false;
        }

        // will be called during the game run each time player collects coins
        public void AddRunEarnings(int amount)
        {
            RunEarnings += amount;
            OnRunEarningsChanged?.Invoke();
            Debug.Log($"Run earnings: {RunEarnings}");
        }

        // when player finishes the run, add the run earnings to the total
        public void FinalizeRunEarnings()
        {
            totalCoins += RunEarnings;
            Debug.Log($"Run earnings {RunEarnings} added to total. New total: {totalCoins}");
            RunEarnings = 0; // Reset earnings
            SaveCoins(); // Ulož nový stav
            OnCoinsChanged?.Invoke();
        }

        /// Saves the total coins to PlayerPrefs.
        private void SaveCoins()
        {
            PlayerPrefs.SetInt(TotalCoinsKey, totalCoins);
            PlayerPrefs.Save(); // Zapíš dáta na disk
            Debug.Log($"[CoinManager] Coins saved: {totalCoins}");
        }

        /// Loads the total coins from PlayerPrefs.
        private void LoadCoins()
        {
            if (PlayerPrefs.HasKey(TotalCoinsKey))
            {
                totalCoins = PlayerPrefs.GetInt(TotalCoinsKey);
                Debug.Log($"[CoinManager] Loaded coins: {totalCoins}");
            }
            else
            {
                totalCoins = initialCoins; // Predvolený poèet coinov
                Debug.Log($"[CoinManager] No saved coins found. Defaulting to {totalCoins}");
            }
        }

        public void ResetCoins()
        {
            LoadCoins();
            OnCoinsChanged?.Invoke();
        }
    }
}
