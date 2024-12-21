using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class for displaying the total coins
    /// </summary>
    public class CoinDisplay : MonoBehaviour
    {
        public Text coinsText; 

        private ICoinManager _coinManager;

        private void Start()
        {
            _coinManager = CoinManager.Instance;

            if (coinsText == null)
            {
                Debug.LogError("[CoinDisplay] Text field for displaying coins is not assigned!");
                return;
            }

            // when the coins change, update the display
            if (_coinManager != null)
            {
                _coinManager.OnCoinsChanged += UpdateCoinDisplay;
            }

            UpdateCoinDisplay();
        }

        private void OnDestroy()
        {
            if (_coinManager != null)
            {
                _coinManager.OnCoinsChanged -= UpdateCoinDisplay;
            }
        }
        
        /// <summary>
        /// Method to update the coin display
        /// </summary>
        private void UpdateCoinDisplay()
        {
            coinsText.text = $"{_coinManager.GetTotalCoins()}";
        }
    }
}
