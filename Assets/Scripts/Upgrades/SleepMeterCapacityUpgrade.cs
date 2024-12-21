using UnityEngine;

namespace Upgrades
{
    /// <summary>
    /// Class implementing the sleep meter capacity upgrade
    /// </summary>
    public class SleepMeterCapacityUpgrade : IUpgrade
    {
        public UpgradeKey Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public string Description { get; private set; }

        public int CurrentLevel { get; private set; }
        
        public float SleepMeterCapacityModifier { get; private set; }
        private float _sleepMeterCapacityIncreasePerLevel;
        public SleepMeterCapacityUpgrade(UpgradeData data)
        {
            if (data == null)
            {
                Debug.LogError("[SleepMeterCapacityUpgrade] UpgradeData is null!");
                return;
            }
            Key = data.key;
            Name = data.title ?? "Unknown Upgrade";
            Icon = data.icon;
            Description = data.description ?? "No Description";
            BasePrice = data.basePrice > 0 ? data.basePrice : 100;
            CurrentLevel = data.currentLevel;
            _sleepMeterCapacityIncreasePerLevel = data.upgradeMultiplier > 0 ? data.upgradeMultiplier : 0.05f;

        }
        
        /// <summary>
        /// Method to apply the upgrade effect
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Sleep meter capacity increased!");
            SleepMeterCapacityModifier = 1 + (CurrentLevel * _sleepMeterCapacityIncreasePerLevel);
        }
        
        /// <summary>
        /// Method to set the upgrade level
        /// </summary>
        /// <param name="level">Level to set</param>
        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
    }
}
