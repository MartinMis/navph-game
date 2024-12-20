using Bosses;
using Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace Controllers
{
    [RequireComponent(typeof(Canvas))]
    public class BossHealthBarController : MonoBehaviour
    {
        [SerializeField] private Image fill;
        [SerializeField] private LampBossController boss;
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
            playerEnterTrigger.OnTriggered += Appear;
            boss.OnDamageTaken += ChangeBar;
        }

        void Appear()
        {
            _canvas.enabled = true;
        }
    
        void ChangeBar()
        {
            fill.fillAmount = boss.Health/boss.MaxHealth;
        }

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
