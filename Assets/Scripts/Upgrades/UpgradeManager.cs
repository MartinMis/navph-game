using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class UpgradeManager : MonoBehaviour
    {
        public static UpgradeManager Instance { get; private set; }

        public GameObject upgradePrefab;
        private Transform contentParent;

        [SerializeField] public List<UpgradeData> upgrades;

        private readonly Dictionary<UpgradeKey, Func<UpgradeData, IUpgrade>> upgradeFactories =
            new Dictionary<UpgradeKey, Func<UpgradeData, IUpgrade>>();

        private Dictionary<UpgradeKey, IUpgrade> createdUpgrades = new Dictionary<UpgradeKey, IUpgrade>();

        private ICoinManager coinManager;

        // it is initialized in HomeScene and blocks are created in UpgradeScene
        private bool isInitialized = false;

        // when this is UpgradeScene, we need to find contentParent where the upgrade blocks will be created
        private const string UpgradeSceneName = "UpgradesScene";
        private const string ContentParentObjectName = "UpgradeContainer";

        public void Initialize()
        {
            Debug.Log("[UpgradeManager] Initializing...");

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            coinManager = CoinManager.Instance;
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
                    Debug.LogError($"[UpgradeManager] UpgradeData has an invalid key: {data.name}");
                    continue;
                }

                var upgrade = CreateUpgrade(data);
                if (upgrade != null)
                {
                    createdUpgrades[data.key] = upgrade;
                }
                else
                {
                    Debug.LogWarning($"[UpgradeManager] Failed to create upgrade for key: {data.key}");
                }
            }

            // Load saved state for all upgrades
            foreach (var upgrade in createdUpgrades.Values)
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

            isInitialized = true;

            SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log("[UpgradeManager] Upgrades loaded into memory, waiting for contentParent to create blocks.");
        }

        public void ReloadUpgrades()
        {
            foreach (var upgrade in createdUpgrades.Values)
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

        private void InitializeFactories()
        {
            upgradeFactories[UpgradeKey.Slippers] = data => new PlayerSpeedUpgrade(data);
            upgradeFactories[UpgradeKey.Bear] = data => new SleepMeterCapacityUpgrade(data);
            upgradeFactories[UpgradeKey.Mask] = data => new SunriseTimerUpgrade(data);
            upgradeFactories[UpgradeKey.Pyjama] = data => new LightDamageUpgrade(data);
        }

        private IUpgrade CreateUpgrade(UpgradeData data)
        {
            Debug.Log($"[UpgradeManager] Processing UpgradeData: {data?.key}");

            if (upgradeFactories.TryGetValue(data.key, out var factory))
            {
                return factory(data);
            }

            Debug.LogWarning($"[UpgradeManager] Unknown upgrade key: {data.key}");
            return null;
        }

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

        public void SetContentParent(Transform parent)
        {
            contentParent = parent;
            Debug.Log("[UpgradeManager] Content parent set from scene load event.");

            TryCreateUpgradeBlocks();
        }

        private void TryCreateUpgradeBlocks()
        {
            if (!isInitialized)
            {
                Debug.LogWarning("[UpgradeManager] Cannot create upgrade blocks, manager not initialized yet.");
                return;
            }

            if (contentParent == null)
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

            foreach (var upgrade in createdUpgrades.Values)
            {
                CreateUpgradeBlock(upgrade);
            }

            Debug.Log("[UpgradeManager] All upgrade blocks created.");
        }

        private void ClearExistingBlocks()
        {
            if (contentParent != null)
            {
                foreach (Transform child in contentParent)
                {
                    Destroy(child.gameObject);
                }
                Debug.Log("[UpgradeManager] Existing upgrade blocks cleared.");
            }
        }

        private void CreateUpgradeBlock(IUpgrade upgrade)
        {
            GameObject upgradeBlockObj = Instantiate(upgradePrefab, contentParent);
            var upgradeBlock = upgradeBlockObj.GetComponent<UpgradeBlock>();

            if (upgradeBlock == null)
            {
                Debug.LogError("[UpgradeManager] UpgradeBlock script is missing on the prefab.");
                return;
            }

            upgradeBlock.Initialize(upgrade.Icon, upgrade.Name, upgrade.BasePrice, upgrade.ApplyEffect, coinManager, upgrade);
        }

        public IUpgrade GetUpgradeByKey(UpgradeKey key)
        {
            if (createdUpgrades.TryGetValue(key, out var upgrade))
            {
                return upgrade;
            }

            Debug.LogWarning($"[UpgradeManager] Upgrade with key '{key}' not found.");
            return null;
        }
    }
}
