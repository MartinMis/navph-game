using UnityEngine;
using Utility;
using Gameplay;

namespace Enemies
{
    /// <summary>
    /// Controller for the alarm clocks
    /// </summary>
    public class AlarmClockController : MonoBehaviour
    {
        [Tooltip("How much damage should the alarm clock deal")]
        [SerializeField] private float damageAmount = 1000f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DamagePlayer(damageAmount, DamageType.Sound);
                }
                else
                {
                    Debug.LogError("[AlarmClockController] PlayerController script not found on the player.");
                }

                Destroy(gameObject);
            }
        }
    }
}
