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
        
        public event Action OnDeath;
        public event Action<int> OnVictory;
        public event Action OnDamageTaken;

        public float Health => _health;
        
        private void Start()
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
            return;
        }
    }
}