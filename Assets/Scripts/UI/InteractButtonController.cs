using System;
using Camera;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Class for the Interact button
    /// </summary>
    public class InteractButtonController : MonoBehaviour
    {
        private Vector3 _targetPosition;
        private Transform _player;
        private FollowPlayer _followingCamera;
        
        /// <summary>
        /// Action triggered when button is clicked
        /// </summary>
        public event Action ButtonPressed;
        
        /// <summary>
        /// Method called when button is clicked
        /// </summary>
        public void ExecuteInteraction()
        {
            ButtonPressed?.Invoke();
            ResetAllListeners();
        }
        
        /// <summary>
        /// Method to reset all listeners so old ones are removed
        /// </summary>
        public void ResetAllListeners()
        {
            ButtonPressed = null;
        }
        
        /// <summary>
        /// Method to check if button already has an assigned interaction
        /// </summary>
        /// <returns>True if interaction is already assigned</returns>
        public bool InteractionIsAssigned()
        {
            if (ButtonPressed == null)
            {
                return false;
            }
            return true;
        }
    
    }
}
