using Managers;
using UnityEngine;
using Upgrades;

namespace UI
{
    /// <summary>
    /// Class implementing the reset button functionality
    /// </summary>
    public class ResetButtonController : MonoBehaviour
    {
        /// <summary>
        /// Reset method
        /// </summary>
        public void Reset()
        {
            PlayerPrefs.DeleteAll();
            CoinManager.Instance.ResetCoins();
            UpgradeManager.Instance.ReloadUpgrades();
        }
    }
}
