using UnityEngine;

namespace Assets.Scripts
{
    public class SleepMeterCapacityUpgrade : IUpgrade
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }

        public int CurrentLevel { get; private set; }
        public SleepMeterCapacityUpgrade(UpgradeData data)
        {
            Key = data.key;
            Name = data.name;
            Icon = data.icon;
            BasePrice = data.basePrice;
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
