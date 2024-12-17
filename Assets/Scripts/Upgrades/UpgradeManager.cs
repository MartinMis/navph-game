using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts
{
    public class UpgradeManager : MonoBehaviour
    {
        public GameObject upgradePrefab;  // UpgradeBlock prefab 
        public Transform contentParent;  // Parent for UI elements ( UpgradeContainer ) 
        public List<UpgradeData> upgrades = new List<UpgradeData>(); // List of upgrades

        private readonly Dictionary<string, Func<UpgradeData, IUpgrade>> upgradeFactories =
            new Dictionary<string, Func<UpgradeData, IUpgrade>>(); // Factory for dynamic creation

        private ICoinManager coinManager;

        private void Awake()

        {
            // used for dependency injection in UpgradeBlock during initialization
            coinManager = CoinManager.Instance;

            // Initialize the factories
            InitializeFactories();
        }

        private void Start()
        {
            if (upgradePrefab == null || contentParent == null)
            {
                Debug.LogError("[UpgradeManager] Missing required components (Prefab/ContentParent).");
                return;
            }

            // Initialize all uprades and their upgrade blocks  
            foreach (var data in upgrades)
            {
                var upgrade = CreateUpgrade(data);
                if (upgrade != null)
                {
                    CreateUpgradeBlock(upgrade);
                }
            }

            Debug.Log("[UpgradeManager] All upgrades initialized.");
        }

        private void InitializeFactories()
        {
            upgradeFactories["slippers"] = data => new PlayerSpeedUpgrade(data);
            upgradeFactories["bear"] = data => new SleepMeterCapacityUpgrade(data);
            upgradeFactories["mask"] = data => new SleepMeterSpeedUpgrade(data);
            upgradeFactories["pyjama"] = data => new LightDamageUpgrade(data);
        }

        // when the game starts, we need to create upgrades from factories
        private IUpgrade CreateUpgrade(UpgradeData data)
        {
            if (upgradeFactories.TryGetValue(data.key, out var factory))
            {
                return factory(data); // Create the upgrade dynamically
            }

            Debug.LogWarning($"[UpgradeManager] Unknown upgrade key: {data.key}");
            return null;
        }

        // when the game starts, we need to create upgrade blocks
        private void CreateUpgradeBlock(IUpgrade upgrade)
        {
            GameObject upgradeBlockObj = Instantiate(upgradePrefab, contentParent);
            var upgradeBlock = upgradeBlockObj.GetComponent<UpgradeBlock>();

            if (upgradeBlock == null)
            {
                Debug.LogError($"[UpgradeManager] UpgradeBlock script is missing on the prefab.");
                return;
            }

            // used to initialize specific data for upgrade block
            upgradeBlock.Initialize(upgrade.Icon, upgrade.Name, upgrade.BasePrice, upgrade.ApplyEffect, coinManager);
        }
    }
}
