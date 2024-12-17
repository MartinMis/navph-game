using UnityEngine;

namespace Assets.Scripts
{
    public class LightDamageUpgrade : IUpgrade
    {
        public UpgradeKey Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }
        public int CurrentLevel { get; private set; }

        public LightDamageUpgrade(UpgradeData data)
        {
            if (data == null)
            {
                Debug.LogError("[LightDamageUpgrade] UpgradeData is null!");
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
            Debug.Log($"{Name}: Light damage reduced!");
            // Logic...
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
    }
}
