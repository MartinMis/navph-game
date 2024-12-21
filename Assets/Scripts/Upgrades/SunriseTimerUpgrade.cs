using UnityEngine;

namespace Upgrades
{
    /// <summary>
    /// Class implementing the sunrise timer upgrade
    /// </summary>
    public class SunriseTimerUpgrade : IUpgrade
    {
        public UpgradeKey Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentLevel { get; private set; }
        
        public string Description { get; private set; }
        
        public float SunriseTimerModifier { get; private set; }
        private float _sunriseTimerModifierIncreasePerLevel;

        public SunriseTimerUpgrade(UpgradeData data)
        {
            if (data == null)
            {
                Debug.LogError("[SleepMeterSpeedUpgrade] UpgradeData is null!");
                return;
            }
            Key = data.key;
            Name = data.title ?? "Unknown Upgrade";
            Icon = data.icon;
            Description = data.description ?? "No Description";
            BasePrice = data.basePrice > 0 ? data.basePrice : 100;
            CurrentLevel = data.currentLevel;
            _sunriseTimerModifierIncreasePerLevel = data.upgradeMultiplier > 0 ? data.upgradeMultiplier : 0.05f;

        }
        
        /// <summary>
        /// Method to apply the upgrade effect
        /// </summary>
        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Sunrise timer increased!");
            SunriseTimerModifier = 1 + (CurrentLevel * _sunriseTimerModifierIncreasePerLevel);
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
