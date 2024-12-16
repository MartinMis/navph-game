using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerSpeedUpgrade : IUpgrade
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentLevel { get; private set; }
        public float SpeedMultiplier { get; private set; }
        public PlayerSpeedUpgrade(UpgradeData data)
        {
            Key = data.key;
            Name = data.name;
            Icon = data.icon;
            BasePrice = data.basePrice;
            CurrentLevel = data.currentLevel;
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Player speed increased!");
            Debug.Log($"{Name}: Player speed level upgrade = {CurrentLevel}!");

            SpeedMultiplier = 1f + (CurrentLevel * 0.05f);

            Debug.Log($"{Name}: Computed Speed Multiplier = {SpeedMultiplier}");
        }
    }
}
