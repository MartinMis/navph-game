using Assets.Scripts;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CoinManager coinManager;
    [SerializeField] private UpgradeManager upgradeManager;

    private void Awake()
    {

        //delete playerprefs
        //PlayerPrefs.DeleteAll();

        Debug.Log("[GameManager] Initializing core managers...");

        // coin manager first, because upgrades depend on coins
        if (coinManager != null)
        {
            coinManager.Initialize();
        }
        else
        {
            Debug.LogError("[GameManager] CoinManager reference is missing!");
        }

        // then upgrade manager
        if (upgradeManager != null)
        {
            upgradeManager.Initialize();
        }
        else
        {
            Debug.LogError("[GameManager] UpgradeManager reference is missing!");
        }

        Debug.Log("[GameManager] Initialization complete.");
    }
}

