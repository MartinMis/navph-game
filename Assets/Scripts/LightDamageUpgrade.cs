using UnityEngine;

namespace Assets.Scripts
{
    public class LightDamageUpgrade : IUpgrade
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }

        public LightDamageUpgrade(UpgradeData data)
        {
            Key = data.key;
            Name = data.name;
            Icon = data.icon;
            BasePrice = data.basePrice;
        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Light damage reduced!");
            // Logic...
        }
    }
}
