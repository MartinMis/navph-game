using System;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace Upgrades
{
    /// <summary>
    /// Implementation of the upgrade block
    /// </summary>
    public class UpgradeBlock : MonoBehaviour, IUpgradeBlock
    {
        public Image upgradeImage;     
        public Text upgradeName;       
        public Text upgradePrice;      
        public Text upgradeCount;      
        public Button upgradeButton;
        public Text upgradeDescription;
        public Image progressBarFill;
        // can be an upgrade description in the future

        private ICoinManager _coinManager;
        private IUpgrade _associatedUpgrade;

        private int _currentUpgradePrice;         
        private int _currentUpgradeLevel;         
        private int _maxUpgradeLevel = 10;
        private int _baseUpgradePrice;
        private int _baseUpgradeLevel = 0;
        public string UpgradeName => upgradeName.text;
        public int CurrentLevel => _currentUpgradeLevel;
        public int MaxLevel => _maxUpgradeLevel;
        public int CurrentPrice => _currentUpgradePrice;


        /// <summary>
        /// Action invoked when the upgrade is applied
        /// </summary>
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

        public void Initialize(Sprite icon, string name, string description, int basePrice, Action onUpgradeApplied, ICoinManager coinManager, IUpgrade upgrade)
        {
            Debug.Log($"[UpgradeBlock] Initializing upgrade: {name}, Base Price: {basePrice}");

            this._coinManager = coinManager ?? throw new ArgumentNullException(nameof(coinManager)); // ensure DI is provided
            
            upgradeImage.sprite = icon; 
            upgradeName.text = name; 
            _currentUpgradePrice = basePrice;
            _baseUpgradePrice = basePrice;
            upgradeDescription.text = description;

            // Invoked in ApplyUpgrade() method
            OnUpgradeApplied = onUpgradeApplied;
            _associatedUpgrade = upgrade;

            LoadUpgradeState();
            UpdateUI();                      

            Debug.Log($"[UpgradeBlock] Initialized {name} successfully");
        }
        
        /// <summary>
        /// Method to apply the upgrade
        /// </summary>
        public void ApplyUpgrade()
        {
            Debug.Log($"[UpgradeBlock] Applying upgrade: {UpgradeName}");
            // when the upgrade is applied, tell CoinManager to update the UI
            OnUpgradeApplied?.Invoke(); 
        }
        
        /// <summary>
        /// Method to reset the upgrades
        /// </summary>
        public void ResetUpgrade()
        {
            _currentUpgradeLevel = _baseUpgradeLevel;
            _currentUpgradePrice = _baseUpgradePrice;
            UpgradeStateStorage.SaveUpgradeState(_associatedUpgrade.Key, _currentUpgradeLevel, _currentUpgradePrice);
            UpdateUI();
        }
        
        /// <summary>
        /// Upgrade next price multiplier
        /// </summary>
        private void NextUpgradePrice()
        {
            _currentUpgradePrice = (int)(_currentUpgradePrice * 1.8);
        }
        
        /// <summary>
        /// When the upgrade buy button is clicked
        /// </summary>
        public void BuyUpgrade()
        {
            Debug.Log($"[UpgradeBlock] Attempting to buy upgrade: {UpgradeName}");
            if (AffordableOrAvailable())
            {
                _currentUpgradeLevel++;
                NextUpgradePrice();

                _associatedUpgrade.SetCurrentLevel(_currentUpgradeLevel);
                //ApplyUpgrade();    
                UpgradeStateStorage.SaveUpgradeState(_associatedUpgrade.Key, _currentUpgradeLevel, _currentUpgradePrice);
                UpdateUI();        
                Debug.Log($"[UpgradeBlock] Upgrade {UpgradeName} purchased. New Level: {_currentUpgradeLevel}");
            }
            else
            {
                Debug.LogWarning($"[UpgradeBlock] Not enough coins or max level reached for {UpgradeName}.");
            }
        }
        
        /// <summary>
        /// Method to check if upgrade is aviable
        /// </summary>
        /// <returns>Upgrade aviability</returns>
        private bool AffordableOrAvailable()
        {
            return _currentUpgradeLevel < _maxUpgradeLevel && _coinManager.SpendCoins(_currentUpgradePrice);
        }
        
        /// <summary>
        /// Method to update the UI
        /// </summary>
        private void UpdateUI()
        {
            upgradePrice.text = $"{_currentUpgradePrice} Coins";
            upgradeCount.text = $"{_currentUpgradeLevel}/{_maxUpgradeLevel}";

            // upgrade progress bar
            if (progressBarFill != null)
            {
                progressBarFill.fillAmount = (float)_currentUpgradeLevel / _maxUpgradeLevel; 
            }

            upgradeButton.interactable = _currentUpgradeLevel < _maxUpgradeLevel; // deactivate buy button on max level


        }
        
        /// <summary>
        /// Loads the saved state of the upgrade from PlayerPrefs.
        /// </summary>
        private void LoadUpgradeState()
        {
            if (UpgradeStateStorage.TryLoadUpgradeState(_associatedUpgrade.Key, out var loadedLevel, out var loadedPrice))
            {
                _currentUpgradeLevel = loadedLevel;
                _currentUpgradePrice = loadedPrice;
                Debug.Log($"[UpgradeBlock] Loaded {UpgradeName}: Level {_currentUpgradeLevel}, Price {_currentUpgradePrice}");
            }
            else
            {
                ResetUpgrade();
                Debug.Log($"[UpgradeBlock] No saved state for {UpgradeName}. Defaulting to Level 0, Price 10");
            }
            _associatedUpgrade.SetCurrentLevel(_currentUpgradeLevel);
        }
    }
}
