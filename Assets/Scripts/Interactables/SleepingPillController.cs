using Gameplay;
using UnityEngine;
using Utility;

namespace Interactables
{
    /// <summary>
    /// Controller for the sleeping pills item
    /// </summary>
    public class SleepingPillController: MonoBehaviour
    {
        [Tooltip("How much health should be restored ")]
        [SerializeField] private float healAmount = 10f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.Heal(healAmount);
                }
                else
                {
                    Debug.LogError("[SleepingPillController] PlayerController script not found on the player.");
                }

                Destroy(gameObject);
            }
        }
    }
}