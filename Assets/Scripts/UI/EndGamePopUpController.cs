using Bosses;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace UI
{
    /// <summary>
    /// Class controlling the end game popup
    /// </summary>
    public class EndGamePopUpController : MonoBehaviour
    {
        [Tooltip("UI elements of the popup")]
        [SerializeField] GameObject uiElements;
        
        [Tooltip("Victory text")]
        [SerializeField] private Text text;
        private GameObject _finalBoss;
        private GameObject _player;
        void Awake()
        {
            _finalBoss = GameObject.FindWithTag(Tags.Boss);
            _player = GameObject.FindWithTag(Tags.Player);
            _player.GetComponent<PlayerController>().OnDeath += DisableOnPlayerDeath;
            _finalBoss.GetComponent<LampBossController>().OnVictory += ToggleVisibility;
        }
        
        /// <summary>
        /// Method for toggling the visibility of the pop up
        /// </summary>
        /// <param name="reward">How many coins was player awarded for defeating the boss</param>
        void ToggleVisibility(int reward = 0)
        {
            text.text = $"Victory! You gain {reward} coins for defeating the boss!";
            uiElements.SetActive(!uiElements.activeSelf);
        }

        void DisableOnPlayerDeath()
        {
            uiElements.SetActive(false);
        }
        
        /// <summary>
        /// Method for returning back to menu after winning
        /// </summary>
        public void BackToMainMenu()
        {
            EndGame.ResetStatsAndEnd();
        }
        
        /// <summary>
        /// Method for continuing the game
        /// </summary>
        public void ContinueGame()
        {
            ToggleVisibility();
            SceneManager.LoadScene("GameScene");
            // Disable the timer and reset the player
            RunTimer.Instance.disabled = true;
            GameObject.FindWithTag(Tags.Player).transform.position = new Vector3(0, -9, 0);
            _finalBoss = GameObject.FindWithTag(Tags.Boss);
        }
        
        // Unsubscribe from the event when destroyed
        void OnDestroy()
        {
            if (_finalBoss != null)
            {
                _finalBoss.GetComponent<LampBossController>().OnVictory -= ToggleVisibility;
            }

            if (_player != null)
            {
                _player.GetComponent<PlayerController>().OnDeath -= DisableOnPlayerDeath;
            }
        }
    }
}
