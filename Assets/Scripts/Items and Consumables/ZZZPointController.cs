using System;
using Managers;
using UnityEngine;

namespace Items_and_Consumables
{
    public class ZZZPointController : MonoBehaviour
    {
        public event Action<ZZZPointController> OnCollected;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                CoinManager.Instance.AddRunEarnings(1);
                Destroy(gameObject);
            }
        }

        private void Collect()
        {
            OnCollected?.Invoke(this);
            Destroy(gameObject);
        }
    }
}
