using Controllers;
using UnityEngine;
using Utility;

namespace UI
{
    public class PlayerDefeatPopUpController : MonoBehaviour
    {
        [SerializeField] private GameObject uiElements;
        void Start()
        {
            GameObject.FindWithTag(Tags.Player).GetComponent<PlayerController>().OnDeath += ToggleVisibility;
        }

        void ToggleVisibility()
        {
            uiElements.SetActive(!uiElements.activeSelf);
        }

        public void ReturnToMainMenu()
        {
            EndGame.ResetStatsAndEnd();
        }

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
