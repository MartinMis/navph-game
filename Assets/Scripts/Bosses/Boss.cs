using System;
using UnityEngine;

namespace Bosses
{
    /// <summary>
    /// Abstract class implementing the basic structure of a boss.
    /// </summary>
    public abstract class Boss: MonoBehaviour
    {   
        [Tooltip("Prefab of the boss")]
        [SerializeField] private GameObject bossPrefab;
        
        [Tooltip("Maximum HP of the boss")]
        [SerializeField] private float maxHealth;
        
        [Tooltip("Number of coins rewarded for defeating the boss")]
        [SerializeField] private int coinReward;
        
        private float _health;
        
        /// <summary>
        /// Action triggered when boss dies.
        /// </summary>
        public event Action OnDeath;
        
        /// <summary>
        /// Action triggered when player achieves victory. Additionally specifies the number of coins rewarded to the
        /// player.
        /// </summary>
        /// <remarks>
        /// Usually triggered at the same time as <c>OnDeath</c> but that depends on the particular boss fight.
        /// </remarks>
        public event Action<int> OnVictory;
        
        /// <summary>
        /// Action invoked whenever the boss takes damage.
        /// </summary>
        public event Action OnDamageTaken;

        public float Health => _health;
        public float MaxHealth => maxHealth;

        void Awake()
        {
            _health = maxHealth;
        }
        
        /// <summary>
        /// Method executed whenever the boss takes damage
        /// </summary>
        /// <param name="damage">Amount of damage to deal to the boss</param>
        public void TakeDamage(float damage)
        {
            _health -= damage;
            OnDamageTaken?.Invoke();
            if (_health <= 0)
            {
                Die();
            }
        }
        
        /// <summary>
        /// Method triggered when boss dies. Automatically calls the appropriate actions, adjust the game difficulty
        /// and adds coins to the player.
        /// </summary>
        private void Die()
        {
            CoinManager.Instance.AddRunEarnings(coinReward);
            DifficultyManager.Instance.IncreaseDifficulty();
            OnDeath?.Invoke();
            OnVictory?.Invoke(coinReward);
            Destroy(gameObject);
        }
    }
}