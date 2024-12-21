using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class for counting the coins during the run
    /// </summary>
    public class RunCoinCounter : MonoBehaviour
    {
        [Tooltip("Text where to display the coin count")]
        [SerializeField] private Text coinText;

        void Start()
        {
            CoinManager.Instance.OnRunEarningsChanged += UpdateCount;
        }
        
        /// <summary>
        /// Method for updating the coin count
        /// </summary>
        void UpdateCount()
        {
            int coinCount = CoinManager.Instance.RunEarnings;
            coinText.text = coinCount.ToString("00");
        }

        void OnDestroy()
        {
            CoinManager.Instance.OnRunEarningsChanged -= UpdateCount;
        }
    }
}
