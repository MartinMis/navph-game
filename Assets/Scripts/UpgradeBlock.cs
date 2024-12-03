using UnityEngine;
using UnityEngine.UI;
using System;

namespace Assets.Scripts
{
    public class UpgradeBlock : MonoBehaviour, IUpgradeBlock
    {
        public Image upgradeImage;     
        public Text upgradeName;       
        public Text upgradePrice;      
        public Text upgradeCount;      
        public Button upgradeButton;
        // can be an upgrade description in the future

        private ICoinManager coinManager;

        private int currentUpgradePrice;         
        private int currentUpgradeLevel;         
        private int maxUpgradeLevel = 10;
        private int baseUpgradePrice = 10;
        private int baseUpgradeLevel = 0;
        public string UpgradeName => upgradeName.text;
        public int CurrentLevel => currentUpgradeLevel;
        public int MaxLevel => maxUpgradeLevel;
        public int CurrentPrice => currentUpgradePrice;



        public Action OnUpgradeApplied { get; set; }

        private void Start()
        {
            Debug.Log($"[UpgradeBlock] Start called for {UpgradeName}");

            // Load upgrade state
            LoadUpgradeState();

            // when the upgrade but button is clicked, the BuyUpgrade method is called
            upgradeButton.onClick.AddListener(BuyUpgrade);

            // initialize UI
            UpdateUI();
        }

        public void Initialize(Sprite icon, string name, int basePrice, Action onUpgradeApplied, ICoinManager coinManager)
        {
            Debug.Log($"[UpgradeBlock] Initializing upgrade: {name}, Base Price: {basePrice}");

            this.coinManager = coinManager ?? throw new ArgumentNullException(nameof(coinManager)); // ensure DI is provided
            
            upgradeImage.sprite = icon; 
            upgradeName.text = name; 
            currentUpgradePrice = basePrice;

            // Invoked in ApplyUpgrade() method
            /** this is IUpgrade ApplyEffect() method which is implemented by
            PlayerSpeedUpgrade, SleepMeterCapacityUpgrade, SleepMeterSpeedUpgrade, LightDamageUpgrade **/
            OnUpgradeApplied = onUpgradeApplied;

            LoadUpgradeState();
            UpdateUI();                      

            Debug.Log($"[UpgradeBlock] Initialized {name} successfully");
        }

        public void ApplyUpgrade()
        {
            Debug.Log($"[UpgradeBlock] Applying upgrade: {UpgradeName}");
            // when the upgrade is applied, tell CoinManager to update the UI
            OnUpgradeApplied?.Invoke(); 
        }

        public void ResetUpgrade()
        {
            currentUpgradeLevel = baseUpgradeLevel;
            currentUpgradePrice = baseUpgradePrice;
            SaveUpgradeState();
            UpdateUI();
        }

        // upgrade next price multiplier
        private void NextUpgradePrice()
        {
            currentUpgradePrice = (int)(currentUpgradePrice * 1.8);
        }

        // when the upgrade buy button is clicked
        public void BuyUpgrade()
        {
            Debug.Log($"[UpgradeBlock] Attempting to buy upgrade: {UpgradeName}");
            if (AffordableOrAvailable())
            {
                currentUpgradeLevel++;
                NextUpgradePrice();
                //ApplyUpgrade();    
                SaveUpgradeState(); 
                UpdateUI();        
                Debug.Log($"[UpgradeBlock] Upgrade {UpgradeName} purchased. New Level: {currentUpgradeLevel}");
            }
            else
            {
                Debug.LogWarning($"[UpgradeBlock] Not enough coins or max level reached for {UpgradeName}.");
            }
        }

        private bool AffordableOrAvailable()
        {
            return currentUpgradeLevel < maxUpgradeLevel && coinManager.SpendCoins(currentUpgradePrice);
        }

        private void UpdateUI()
        {
            upgradePrice.text = $"{currentUpgradePrice} Coins";
            upgradeCount.text = $"{currentUpgradeLevel}/{maxUpgradeLevel}";
            upgradeButton.interactable = currentUpgradeLevel < maxUpgradeLevel; // deactivate buy button on max level
        }

        // saves the current state of the upgrade to PlayerPrefs.
        private void SaveUpgradeState()
        {
            string keyLevel = $"Upgrade_{UpgradeName}_Level";
            string keyPrice = $"Upgrade_{UpgradeName}_Price";

            PlayerPrefs.SetInt(keyLevel, currentUpgradeLevel);
            PlayerPrefs.SetInt(keyPrice, currentUpgradePrice);
            PlayerPrefs.Save();

            Debug.Log($"[UpgradeBlock] Saved {UpgradeName}: Level {currentUpgradeLevel}, Price {currentUpgradePrice}");
        }

        /// loads the saved state of the upgrade from PlayerPrefs.
        private void LoadUpgradeState()
        {
            string keyLevel = $"Upgrade_{UpgradeName}_Level";
            string keyPrice = $"Upgrade_{UpgradeName}_Price";

            if (PlayerPrefs.HasKey(keyLevel) && PlayerPrefs.HasKey(keyPrice))
            {
                currentUpgradeLevel = PlayerPrefs.GetInt(keyLevel);
                currentUpgradePrice = PlayerPrefs.GetInt(keyPrice);
                Debug.Log($"[UpgradeBlock] Loaded {UpgradeName}: Level {currentUpgradeLevel}, Price {currentUpgradePrice}");
            }
            else
            {
                ResetUpgrade();
                Debug.Log($"[UpgradeBlock] No saved state for {UpgradeName}. Defaulting to Level 0, Price 10");
            }
        }
    }
}
