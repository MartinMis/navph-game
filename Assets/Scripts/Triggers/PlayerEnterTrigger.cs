using System;
using Utility;
using UnityEngine;


namespace Triggers
{
    /// <summary>
    /// Class containing a trigger for when player enters an area
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Collider2D))]
    public class PlayerEnterTrigger : MonoBehaviour
    {
        /// <summary>
        /// Triggered when player enters
        /// </summary>
        public event Action OnTriggered;
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                OnTriggered?.Invoke();
            }
        }
    }
}

