using System.Collections;
using Gameplay;
using Unity.VisualScripting;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Controller for the couch
    /// </summary>
    public class CouchController : Interactable
    {
        [Tooltip("How long is player forced to sit on the couch")]
        [SerializeField] private float restTime = 10f;
        
        [Tooltip("How much should the couch heal")]
        [SerializeField] private float reductionAmount = 50;
        
        /// <summary>
        /// Override of <c>Interact</c> method
        /// </summary>
        /// <param name="player">PlayerController refernece</param>
        public override void Interact(PlayerController player)
        {
            // Save the player position, move him to couch and make him invincible
            var originalPlayerPosition = player.transform.position;
            player.ToggleMovement();
            player.GameObject().GetComponent<CapsuleCollider2D>().enabled = false;
            player.transform.position = transform.position;
            player.godMode = true;
            Debug.Log("[CouchController] Interact");
            // Start a coroutine to reset the player back
            StartCoroutine(PerformRest(player, restTime, reductionAmount, originalPlayerPosition));
        }
        
        /// <summary>
        /// Coroutine to reset the player. Wait system inspired by
        /// https://discussions.unity.com/t/how-to-wait-a-certain-amount-of-seconds-in-c/192244
        /// </summary>
        /// <param name="player">Reference to the player</param>
        /// <param name="timeRequired">How long to wait before resetting the player</param>
        /// <param name="restoreAmount">How much to heal the player</param>
        /// <param name="restorePosition">Position where to restore the player to</param>
        IEnumerator PerformRest(PlayerController player, float timeRequired, float restoreAmount, Vector3 restorePosition)
        {
            // Wait for the given time
            Debug.Log($"[CouchController] Performing rest for {timeRequired} seconds");
            yield return new WaitForSeconds(timeRequired);
            // Restore player and heal them
            player.transform.position = restorePosition;
            player.GameObject().GetComponent<CapsuleCollider2D>().enabled = true;
            player.Heal(restoreAmount);
            player.godMode = false;
            player.ToggleMovement();
        }
    }
}
