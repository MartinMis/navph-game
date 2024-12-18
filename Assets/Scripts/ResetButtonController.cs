using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;

public class ResetButtonController : MonoBehaviour
{
    public void Reset()
    {
        PlayerPrefs.DeleteAll();
        CoinManager.Instance.ResetCoins();
        UpgradeManager.Instance.ReloadUpgrades();
    }
}
