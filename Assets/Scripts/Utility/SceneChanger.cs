using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    /// <summary>
    /// Helper class for changing the scenes
    /// </summary>
    public class SceneChanger : MonoBehaviour
    {
        /// <summary>
        /// Load Help scene
        /// </summary>
        public void LoadHelp()
        {
            SceneManager.LoadScene("Help");
        }
        
        /// <summary>
        /// Load Game scene
        /// </summary>
        public void LoadGame()
        {
            SceneManager.LoadScene("GameScene");
        }
        
        /// <summary>
        /// Load Credits scene
        /// </summary>
        public void LoadCredits()
        {
            SceneManager.LoadScene("Credits");
        }
        
        /// <summary>
        /// Load Home scene
        /// </summary>
        public void LoadHome()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}

