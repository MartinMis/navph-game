using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
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
