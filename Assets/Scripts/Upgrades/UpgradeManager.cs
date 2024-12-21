using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Upgrades
{
    /// <summary>
    /// Class managing the upgrades implemented as a singleton
    /// </summary>
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance { get; private set; }
        public List<UpgradeData> upgrades;
        public GameObject upgradePrefab;
       
        private Transform _contentParent;
        private readonly Dictionary<UpgradeKey, Func<UpgradeData, IUpgrade>> _upgradeFactories =
            new Dictionary<UpgradeKey, Func<UpgradeData, IUpgrade>>();
        private Dictionary<UpgradeKey, IUpgrade> _createdUpgrades = new Dictionary<UpgradeKey, IUpgrade>();
        private ICoinManager _coinManager;

        // it is initialized in HomeScene and blocks are created in UpgradeScene
        private bool _isInitialized = false;

        // when this is UpgradeScene, we need to find contentParent where the upgrade blocks will be created
        private const string UpgradeSceneName = "UpgradesScene";
        private const string ContentParentObjectName = "UpgradeContainer";
        
        /// <summary>
        /// Method for initializing the upgrade manager singleton
        /// </summary>
        public void Initialize()
        {
            Debug.Log("[UpgradeManager] Initializing...");

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _coinManager = CoinManager.Instance;
            InitializeFactories();

            // Initialize all upgrades
            foreach (var data in upgrades)
            {
                if (data == null)
                {
                    Debug.LogError("[UpgradeManager] Found null UpgradeData in the list!");
                    continue;
                }

                if (string.IsNullOrEmpty(data.key.ToString()))
                {
                    Debug.LogError($"[UpgradeManager] UpgradeData has an invalid key: {data.title}");
                    continue;
                }

                var upgrade = CreateUpgrade(data);
                if (upgrade != null)
                {
                    _createdUpgrades[data.key] = upgrade;
                }
                else
                {
                    Debug.LogWarning($"[UpgradeManager] Failed to create upgrade for key: {data.key}");
                }
            }

            // Load saved state for all upgrades
            foreach (var upgrade in _createdUpgrades.Values)
            {
                if (UpgradeStateStorage.TryLoadUpgradeState(upgrade.Key, out var level, out var price))
                {
                    upgrade.SetCurrentLevel(level);
                    Debug.Log($"[UpgradeManager] Loaded {upgrade.Key}: Level {level}, Price {price} into IUpgrade.");
                }
                else
                {
                    upgrade.SetCurrentLevel(0);
                    Debug.Log($"[UpgradeManager] No saved state for {upgrade.Key}. Using default Level 0.");
                }
            }

            _isInitialized = true;

            SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log("[UpgradeManager] Upgrades loaded into memory, waiting for contentParent to create blocks.");
        }
        
        /// <summary>
        /// Method for reloading the upgrades
        /// </summary>
        public void ReloadUpgrades()
        {
            foreach (var upgrade in _createdUpgrades.Values)
            {
                if (UpgradeStateStorage.TryLoadUpgradeState(upgrade.Key, out var level, out var price))
                {
                    upgrade.SetCurrentLevel(level);
                    Debug.Log($"[UpgradeManager] Loaded {upgrade.Key}: Level {level}, Price {price} into IUpgrade.");
                }
                else
                {
                    upgrade.SetCurrentLevel(0);
                    Debug.Log($"[UpgradeManager] No saved state for {upgrade.Key}. Using default Level 0.");
                }
            }
        }
        
        /// <summary>
        /// Helper method initializing the upgrade fcctories
        /// </summary>
        private void InitializeFactories()
        {
            _upgradeFactories[UpgradeKey.Slippers] = data => new PlayerSpeedUpgrade(data);
            _upgradeFactories[UpgradeKey.Bear] = data => new SleepMeterCapacityUpgrade(data);
            _upgradeFactories[UpgradeKey.Mask] = data => new SunriseTimerUpgrade(data);
            _upgradeFactories[UpgradeKey.Pyjama] = data => new LightDamageUpgrade(data);
        }
        
        /// <summary>
        /// Helper method for creating the upgrades from upgrade data
        /// </summary>
        /// <param name="data">Upgrade data</param>
        /// <returns>Upgrade</returns>
        private IUpgrade CreateUpgrade(UpgradeData data)
        {
            Debug.Log($"[UpgradeManager] Processing UpgradeData: {data?.key}");

            if (_upgradeFactories.TryGetValue(data.key, out var factory))
            {
                return factory(data);
            }

            Debug.LogWarning($"[UpgradeManager] Unknown upgrade key: {data.key}");
            return null;
        }
        
        /// <summary>
        /// Method to be executed when scene changes
        /// </summary>
        /// <param name="scene">Name of the loaded scene</param>
        /// <param name="mode">Scene mode</param>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == UpgradeSceneName)
            {
                Debug.Log("[UpgradeManager] UpgradeScene loaded. Trying to find contentParent...");

                var parentObj = GameObject.Find(ContentParentObjectName);
                if (parentObj != null)
                {
                    SetContentParent(parentObj.transform);
                }
                else
                {
                    Debug.LogWarning("[UpgradeManager] Could not find contentParent in UpgradeScene!");
                }
            }
        }
        
        /// <summary>
        /// Method to set parents
        /// </summary>
        /// <param name="parent">Object to act as a parrent</param>
        private void SetContentParent(Transform parent)
        {
            _contentParent = parent;
            Debug.Log("[UpgradeManager] Content parent set from scene load event.");

            TryCreateUpgradeBlocks();
        }
        
        /// <summary>
        /// Method to try and create upgrade blocks
        /// </summary>
        private void TryCreateUpgradeBlocks()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("[UpgradeManager] Cannot create upgrade blocks, manager not initialized yet.");
                return;
            }

            if (_contentParent == null)
            {
                Debug.LogWarning("[UpgradeManager] Cannot create upgrade blocks, contentParent not set.");
                return;
            }

            if (upgradePrefab == null)
            {
                Debug.LogError("[UpgradeManager] Missing upgradePrefab.");
                return;
            }

            ClearExistingBlocks();

            foreach (var upgrade in _createdUpgrades.Values)
            {
                CreateUpgradeBlock(upgrade);
            }

            Debug.Log("[UpgradeManager] All upgrade blocks created.");
        }
        
        /// <summary>
        /// Method to remove already existing upgrade blocks
        /// </summary>
        private void ClearExistingBlocks()
        {
            if (_contentParent != null)
            {
                foreach (Transform child in _contentParent)
                {
                    Destroy(child.gameObject);
                }
                Debug.Log("[UpgradeManager] Existing upgrade blocks cleared.");
            }
        }
        
        /// <summary>
        /// Method to create upgrade block for an upgrade
        /// </summary>
        /// <param name="upgrade">Upgrade to create upgrade block for</param>
        private void CreateUpgradeBlock(IUpgrade upgrade)
        {
            GameObject upgradeBlockObj = Instantiate(upgradePrefab, _contentParent);
            var upgradeBlock = upgradeBlockObj.GetComponent<UpgradeBlock>();

            if (upgradeBlock == null)
            {
                Debug.LogError("[UpgradeManager] UpgradeBlock script is missing on the prefab.");
                return;
            }

            upgradeBlock.Initialize(upgrade.Icon, upgrade.Name, upgrade.Description, upgrade.BasePrice, upgrade.ApplyEffect, _coinManager, upgrade);
        }
        
        /// <summary>
        /// Method to get upgrade by its key
        /// </summary>
        /// <param name="key">Key of the upgrade</param>
        /// <returns>Upgrade</returns>
        public IUpgrade GetUpgradeByKey(UpgradeKey key)
        {
            if (_createdUpgrades.TryGetValue(key, out var upgrade))
            {
                return upgrade;
            }

            Debug.LogWarning($"[UpgradeManager] Upgrade with key '{key}' not found.");
            return null;
        }
    }
}
