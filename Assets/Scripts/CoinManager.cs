using Assets.Scripts;
using System;
using UnityEngine;

public class CoinManager : MonoBehaviour, ICoinManager
{
    // Singleton instance
    

    private int totalCoins; // global total coins 
    private int runEarnings = 0;  // in game run earnings

    public event Action OnCoinsChanged;

    private const string TotalCoinsKey = "TotalCoins"; // for PlayerPrefs
    public static ICoinManager Instance { get; private set; }
    private void Awake()
    {

        //PlayerPrefs.DeleteAll(); // reset coins and other player prefs

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

        // Load coins from PlayerPrefs
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
        runEarnings += amount;
        Debug.Log($"Run earnings: {runEarnings}");
    }

    // when player finishes the run, add the run earnings to the total
    public void FinalizeRunEarnings()
    {
        totalCoins += runEarnings;
        Debug.Log($"Run earnings {runEarnings} added to total. New total: {totalCoins}");
        runEarnings = 0; // Reset earnings
        SaveCoins(); // Ulo� nov� stav
        OnCoinsChanged?.Invoke();
    }

    /// Saves the total coins to PlayerPrefs.
    private void SaveCoins()
    {
        PlayerPrefs.SetInt(TotalCoinsKey, totalCoins);
        PlayerPrefs.Save(); // Zap� d�ta na disk
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
            totalCoins = 10000; // Predvolen� po�et coinov
            Debug.Log($"[CoinManager] No saved coins found. Defaulting to {totalCoins}");
        }
    }
}
