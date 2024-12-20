using Controllers;
using UnityEngine;
using Utility;

namespace Items_and_Consumables
{
    public class CoffeeCupController : MonoBehaviour
    {
        [SerializeField] private float damageAmount = 10f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DamagePlayer(damageAmount, DamageType.Coffee);
                }
                else
                {
                    Debug.LogError("PlayerController script not found on the player.");
                }

                Destroy(gameObject);
            }
        }
    }
}
