using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CoinDisplay : MonoBehaviour
    {
        public Text coinsText; 

        private ICoinManager coinManager;

        private void Start()
        {
            coinManager = CoinManager.Instance;

            if (coinsText == null)
            {
                Debug.LogError("[CoinDisplay] Text field for displaying coins is not assigned!");
                return;
            }

            // when the coins change, update the display
            if (coinManager != null)
            {
                coinManager.OnCoinsChanged += UpdateCoinDisplay;
            }

            UpdateCoinDisplay();
        }

        private void OnDestroy()
        {
            if (coinManager != null)
            {
                coinManager.OnCoinsChanged -= UpdateCoinDisplay;
            }
        }

        private void UpdateCoinDisplay()
        {
            coinsText.text = $"{coinManager.GetTotalCoins()}";
        }
    }
}
