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

        public List<UpgradeData> upgrades = new List<UpgradeData>();

        private readonly Dictionary<string, Func<UpgradeData, IUpgrade>> upgradeFactories =
            new Dictionary<string, Func<UpgradeData, IUpgrade>>();

        private Dictionary<string, IUpgrade> createdUpgrades = new Dictionary<string, IUpgrade>();

        private ICoinManager coinManager;

        // it is initialized in HomeScene and blocks are created in UpgradeScene
        private bool isInitialized = false;
        private bool blocksCreated = false;

        // when this is UpgradeScene, we need to find contentParent where the upgrade blocks will be created
        private string upgradeSceneName = "UpgradesScene";
        // contentParent is the parent object of all upgrade blocks
        private string contentParentObjectName = "UpgradeCointainer";

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


            // initialize all upgrades
            foreach (var data in upgrades)
            {
                var upgrade = CreateUpgrade(data);
                if (upgrade != null)
                {
                    createdUpgrades[upgrade.Key] = upgrade;
                }
            }

            // load saved state for all upgrades
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

            // Zaregistrujeme sa na udalosť, ktorá sa zavolá, keď sa načíta nová scéna
            SceneManager.sceneLoaded += OnSceneLoaded;

            Debug.Log("[UpgradeManager] Upgrades loaded into memory, waiting for contentParent to create blocks.");
        }

        private void InitializeFactories()
        {
            upgradeFactories["slippers"] = data => new PlayerSpeedUpgrade(data);
            upgradeFactories["bear"] = data => new SleepMeterCapacityUpgrade(data);
            upgradeFactories["mask"] = data => new SleepMeterSpeedUpgrade(data);
            upgradeFactories["pyjama"] = data => new LightDamageUpgrade(data);
        }

        private IUpgrade CreateUpgrade(UpgradeData data)
        {
            if (upgradeFactories.TryGetValue(data.key, out var factory))
            {
                return factory(data);
            }

            Debug.LogWarning($"[UpgradeManager] Unknown upgrade key: {data.key}");
            return null;
        }

        // Callback pri načítaní scény
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Skontrolujeme, či je to UpgradeScene
            if (scene.name == "UpgradesScene")
            {
                Debug.Log("[UpgradeManager] UpgradeScene loaded. Trying to find contentParent...");
                // Nájdeme contentParent podľa názvu
                var parentObj = GameObject.Find("UpgradeContainer");
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

            if (blocksCreated)
            {
                Debug.LogWarning("[UpgradeManager] Upgrade blocks already created.");
                return;
            }

            foreach (var upgrade in createdUpgrades.Values)
            {
                CreateUpgradeBlock(upgrade);
            }

            blocksCreated = true;
            Debug.Log("[UpgradeManager] All upgrade blocks created.");
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

        // when I want to apply an upgrade, I will find it by upgrade key [slippers, bear, mask, pyjama]
        public IUpgrade GetUpgradeByKey(string key)
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
