using UnityEngine;
using Upgrades;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private CoinManager coinManager;
        [SerializeField] private UpgradeManager upgradeManager;
    
        public static GameManager Instance {get; private set;}
    
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (Instance != this)
            {
                Destroy(gameObject);
            }
        
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
}

