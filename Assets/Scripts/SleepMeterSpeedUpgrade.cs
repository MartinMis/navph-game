using UnityEngine;

namespace Assets.Scripts
{
    public class SleepMeterSpeedUpgrade : IUpgrade
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentLevel { get; private set; }

        public SleepMeterSpeedUpgrade(UpgradeData data)
        {
            Key = data.key;
            Name = data.name;
            Icon = data.icon;
            BasePrice = data.basePrice;
            CurrentLevel = data.currentLevel;
        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Sleep meter regeneration speed increased!");
            // Logic...
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
    }
}
