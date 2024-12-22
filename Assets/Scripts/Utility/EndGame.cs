using Gameplay;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    /// <summary>
    /// Helper class to handle the end of the game
    /// </summary>
    public class EndGame
    {
        /// <summary>
        /// Method to destroy and reset necessary objects
        /// </summary>
        public static void ResetStatsAndEnd()
        {
            // UI manager is persistent between game scene but we need to destroy it when going to menu
            Object.Destroy(UIManager.Instance.gameObject);
            Object.Destroy(DifficultyManager.Instance.gameObject);
            RunTimer.Instance.disabled = false;
            CoinManager.Instance.FinalizeRunEarnings();
            var player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player != null)
            { 
                Object.Destroy(player);
            }
            SceneManager.LoadScene("HomeScene");
        }
    }
}