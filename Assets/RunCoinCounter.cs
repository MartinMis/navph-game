using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
}
