using UnityEngine;

namespace Assets.Scripts
{
    public class PlayerSpeedUpgrade : IUpgrade
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public Sprite Icon { get; private set; }
        public int BasePrice { get; private set; }

        public PlayerSpeedUpgrade(UpgradeData data)
        {
            Key = data.key;
            Name = data.name;
            Icon = data.icon;
            BasePrice = data.basePrice;
        }

        public void ApplyEffect()
        {
            Debug.Log($"{Name}: Player speed increased!");
            // Logic...
        }
    }
}
