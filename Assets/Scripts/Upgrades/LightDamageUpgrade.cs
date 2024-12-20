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

        public string Description { get; private set; }
        
        public float LightDamageModifier { get; private set; }
        private float lightDamageDecreasePerLevel;

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
            Description = data.description ?? "No Description";
            BasePrice = data.basePrice > 0 ? data.basePrice : 100;
            CurrentLevel = data.currentLevel;
            lightDamageDecreasePerLevel = data.upgradeMultiplier > 0 ? data.upgradeMultiplier : 0.05f;
        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Light damage reduced!");
            LightDamageModifier = 1 - (CurrentLevel*lightDamageDecreasePerLevel); 
        }

        public void SetCurrentLevel(int level)
        {
            CurrentLevel = level;
        }
    }
}
