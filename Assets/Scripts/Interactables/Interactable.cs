using Gameplay;
using UnityEngine;

namespace Interactables
{
    /// <summary>
    /// Interactable abstract class
    /// </summary>
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        /// Range in which item can be interacted with
        /// </summary>
        public float interactionRadius = 3;
        
        /// <summary>
        /// Abstract interaction method
        /// </summary>
        /// <param name="player">PlayerController reference</param>
        public abstract void Interact(PlayerController player);
    
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, interactionRadius);
        }
    }
}