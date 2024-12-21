using UnityEngine;

namespace Upgrades
{
    /// <summary>
    /// Class implementing the player speed upgrade
    /// </summary>
    public class PlayerSpeedUpgrade : IUpgrade
    {
        public UpgradeKey Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentLevel { get; private set; }
        public float SpeedMultiplier { get; private set; }
        public string Description { get; private set; }

        private float _speedIncreasePerLevel;

        public PlayerSpeedUpgrade(UpgradeData data)
        {
            if (data == null)
            {
                Debug.LogError("[PlayerSpeedUpgrade] UpgradeData is null!");
                return;
            }

            Key = data.key;
            Name = data.title ?? "Unknown Upgrade";
            Icon = data.icon;
            Description = data.description ?? "No Description";
            BasePrice = data.basePrice > 0 ? data.basePrice : 100; // Default price
            CurrentLevel = data.currentLevel;
            _speedIncreasePerLevel = data.upgradeMultiplier > 0 ? data.upgradeMultiplier : 0.05f;
        }
        
        /// <summary>
        /// Method to set the upgrade level
        /// </summary>
        /// <param name="level">Level to set</param>
        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
        
        /// <summary>
        /// Method to apply the upgrade effect
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Player speed increased!");
            Debug.Log($"{Name}: Player speed level upgrade = {CurrentLevel}!");

            SpeedMultiplier = 1f + (CurrentLevel * _speedIncreasePerLevel);

            Debug.Log($"{Name}: Computed Speed Multiplier = {SpeedMultiplier}");
        }
    }
}
