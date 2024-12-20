using System;
using UnityEngine;

namespace Bosses
{
    public abstract class Boss: MonoBehaviour
    {   
        [SerializeField] private GameObject bossPrefab;
        [SerializeField] private float maxHealth;
        [SerializeField] private int coinReward;
        private float _health;
        
        /// <summary>
        /// Action triggered when boss dies.
        /// </summary>
        public event Action OnDeath;
        
        /// <summary>
        /// Action triggered when player achieves victory.
        /// </summary>
        /// <remarks>
        /// Usually triggered at the same time
        /// </remarks>
        public event Action<int> OnVictory;
        public event Action OnDamageTaken;

        public float Health => _health;
        public float MaxHealth => maxHealth;

        void Awake()
        {
            _health = maxHealth;
        }
        
        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnDamageTaken?.Invoke();
            if (_health <= 0)
            {
                Die();
            }
        }

        private void Die()
        {
            CoinManager.Instance.AddCoins(coinReward);
            DifficultyManager.Instance.IncreaseDifficulty();
            OnDeath?.Invoke();
            OnVictory?.Invoke(coinReward);
            Destroy(gameObject);
        }
    }
}