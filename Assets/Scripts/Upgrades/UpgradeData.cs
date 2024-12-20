using UnityEngine;

namespace Upgrades
{
    [CreateAssetMenu(fileName = "NewUpgradeData", menuName = "Upgrade/Upgrade Data")]
    public class UpgradeData : ScriptableObject
    {
        public UpgradeKey key;
        public string name;
        public string description;
        public Sprite icon;
        public int basePrice;
        public int currentLevel;
        public float upgradeMultiplier = 0.05f;
    }
}
