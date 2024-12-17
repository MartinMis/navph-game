﻿using UnityEngine;

namespace Assets.Scripts
{
    public class SleepMeterCapacityUpgrade : IUpgrade
    {
        public UpgradeKey Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }

        public int CurrentLevel { get; private set; }
        public SleepMeterCapacityUpgrade(UpgradeData data)
        {
            if (data == null)
            {
                Debug.LogError("[SleepMeterCapacityUpgrade] UpgradeData is null!");
                return;
            }
            Key = data.key;
            Name = data.name ?? "Unknown Upgrade";
            Icon = data.icon;
            BasePrice = data.basePrice > 0 ? data.basePrice : 100;
            CurrentLevel = data.currentLevel;

        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Sleep meter capacity increased!");
            // Logic...
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
    }
}
