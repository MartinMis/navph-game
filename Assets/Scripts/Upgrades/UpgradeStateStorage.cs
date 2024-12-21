using UnityEngine;

namespace Upgrades
{
    /// <summary>
    /// Class for storing the upgrade states
    /// </summary>
    public static class UpgradeStateStorage
    {
        /// <summary>
        /// When the game starts, we need to load the state of all upgrades (level and price)
        /// </summary>
        public static bool TryLoadUpgradeState(UpgradeKey upgradeKey, out int level, out int price)
        {
            string keyLevel = $"Upgrade_{upgradeKey}_Level";
            string keyPrice = $"Upgrade_{upgradeKey}_Price";

            if (PlayerPrefs.HasKey(keyLevel) && PlayerPrefs.HasKey(keyPrice))
            {
                level = PlayerPrefs.GetInt(keyLevel);
                price = PlayerPrefs.GetInt(keyPrice);
                return true;
            }

            level = 0;
            price = 0;
            return false;
        }
        
        /// <summary>
        /// When the player buys an upgrade, we need to save the new state
        /// </summary>
        public static void SaveUpgradeState(UpgradeKey upgradeKey, int level, int price)
        {
            string keyLevel = $"Upgrade_{upgradeKey}_Level";
            string keyPrice = $"Upgrade_{upgradeKey}_Price";

            PlayerPrefs.SetInt(keyLevel, level);
            PlayerPrefs.SetInt(keyPrice, price);
            PlayerPrefs.Save();
        }
    }
}
