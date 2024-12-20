using UnityEngine;

namespace Upgrades
{
    public static class UpgradeStateStorage
    {
        // when the game starts, we need to load the state of all upgrades (level and price)
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

        // when the player buys an upgrade, we need to save the new state
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
