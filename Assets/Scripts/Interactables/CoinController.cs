using System;
using Managers;
using UnityEngine;
using Utility;

namespace Interactables
{
    /// <summary>
    /// Class implementing the Coin controller
    /// </summary>
    public class CoinController : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                CoinManager.Instance.AddRunEarnings(1);
                Destroy(gameObject);
            }
        }
        
    }
}
