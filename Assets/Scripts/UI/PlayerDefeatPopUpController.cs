using Gameplay;
using UnityEngine;
using Utility;

namespace UI
{
    /// <summary>
    /// Controller for player defeat pop ip
    /// </summary>
    public class PlayerDefeatPopUpController : MonoBehaviour
    {
        [Tooltip("UI elements of the pop up")]
        [SerializeField] private GameObject uiElements;
        void Start()
        {
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().OnDeath += ToggleVisibility;
        }
        
        /// <summary>
        /// Method to toggle pop up visibility
        /// </summary>
        void ToggleVisibility()
        {
            uiElements.SetActive(!uiElements.activeSelf);
        }
        
        /// <summary>
        /// Method for returning to menu
        /// </summary>
        public void ReturnToMainMenu()
        {
            EndGame.ResetStatsAndEnd();
        }
        
        // Unsubscribe from event when destroyed
        void OnDestroy()
        {
            var playerController = GameObject.FindWithTag(Tags.Player)?.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.OnDeath -= ToggleVisibility;
            }
        }
    
    }
}
