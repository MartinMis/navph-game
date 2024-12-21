using Gameplay;
using UnityEngine;
using Utility;

namespace Interactables
{
    /// <summary>
    /// Controller for the coffee cups
    /// </summary>
    public class CoffeeCupController : MonoBehaviour
    {
        [Tooltip("How much damage to deal")]
        [SerializeField] private float damageAmount = 10f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DamagePlayer(damageAmount, DamageType.Coffee);
                }
                else
                {
                    Debug.LogError("[CoffeeCupController] PlayerController script not found on the player.");
                }

                Destroy(gameObject);
            }
        }
    }
}
