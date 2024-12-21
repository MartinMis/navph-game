using UnityEngine;
using Upgrades;

namespace Managers
{
    /// <summary>
    /// Class centralising the core managers
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        [Tooltip("Coin manger to use")]
        [SerializeField] private CoinManager coinManager;
        
        [Tooltip("Upgrade manager to use")]
        [SerializeField] private UpgradeManager upgradeManager;
        
        [Tooltip("Audio manager to use")]
        [SerializeField] private AudioManager audioManager;
    
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
            
            // then audio manager
            if (audioManager != null)
            {
                audioManager.Initialize();
            }
            else
            {
                Debug.LogError("[GameManager] AudioManager reference is missing!");
            }

            Debug.Log("[GameManager] Initialization complete.");
        }
    }
}

