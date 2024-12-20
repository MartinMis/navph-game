using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class RunCoinCounter : MonoBehaviour
    {
        [SerializeField] private Text coinText;

        void Start()
        {
            CoinManager.Instance.OnRunEarningsChanged += UpdateCount;
        }
    
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
