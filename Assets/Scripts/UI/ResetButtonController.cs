using Managers;
using UnityEngine;
using Upgrades;

namespace UI
{
    public class ResetButtonController : MonoBehaviour
    {
        public void Reset()
        {
            PlayerPrefs.DeleteAll();
            CoinManager.Instance.ResetCoins();
            UpgradeManager.Instance.ReloadUpgrades();
        }
    }
}
