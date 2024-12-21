using Bosses;
using Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class for managing the boss healthbar
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public class BossHealthBarController : MonoBehaviour
    {
        [Tooltip("Fill part of the healthbar")]
        [SerializeField] private Image fill;
        
        [Tooltip("Boss whose HP should be displayed")]
        [SerializeField] private LampBossController boss;
        
        [Tooltip("Trigger for the healthbar appearing")]
        [SerializeField] private PlayerEnterTrigger playerEnterTrigger;
    
        private Canvas _canvas;
    
        void Start()
        {
            _canvas = GetComponent<Canvas>();
            if (_canvas == null)
            {
                Debug.LogError("[BossHealthBarController] Canvas not found");
                return;
            }
            _canvas.enabled = false;
            // Subscribe to actions
            playerEnterTrigger.OnTriggered += Appear;
            boss.OnDamageTaken += ChangeBar;
        }
        
        /// <summary>
        /// Method to show the healthbat
        /// </summary>
        void Appear()
        {
            _canvas.enabled = true;
        }
        
        /// <summary>
        /// Method to update the healtbar state by the current boss hp
        /// </summary>
        void ChangeBar()
        {
            fill.fillAmount = boss.Health/boss.MaxHealth;
        }
        
        // Unsubscribe from actions when destroyed
        void OnDestroy()
        {
            if (playerEnterTrigger != null)
            {
                playerEnterTrigger.OnTriggered -= Appear;
            }

            if (boss != null)
            {
                boss.OnDamageTaken -= ChangeBar;
            }
        }
    }
}
