using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    /// <summary>
    /// Class controlling the player navigation 
    /// </summary>
    public class TabNavigationController : MonoBehaviour
    {
        // list of all buttons with configuration
        public List<Tab> tabs; 

        void Start()
        {
            string currentScene = SceneManager.GetActiveScene().name;

            // set button states based on current scene
            foreach (var tab in tabs)
            {
                bool isActive = tab.config.targetScene == currentScene;
                UpdateButtonState(tab, isActive);

                // button on click listener
                tab.button.onClick.AddListener(() => LoadScene(tab.config.targetScene));
            }
        }
        
        /// <summary>
        /// Method for updating the button state
        /// </summary>
        /// <param name="tab">Tab to update</param>
        /// <param name="isActive">If given button is active</param>
        private void UpdateButtonState(Tab tab, bool isActive)
        {
            UpdateButtonVisuals(tab, isActive);
            UpdateButtonInteractivity(tab, isActive);
        }
        
        /// <summary>
        /// Method for updating the button visuals
        /// </summary>
        /// <param name="tab">Tab to update</param>
        /// <param name="isActive">If given button is active</param>
        private void UpdateButtonVisuals(Tab tab, bool isActive)
        {
            // swap sprite
            Image buttonImage = tab.button.GetComponent<Image>();
            buttonImage.sprite = isActive ? tab.config.activeSprite : tab.config.normalSprite;
        }
        
        /// <summary>
        /// Method for updating the button interactivity
        /// </summary>
        /// <param name="tab">Tab to update</param>
        /// <param name="isActive">If given button is active</param>
        private void UpdateButtonInteractivity(Tab tab, bool isActive)
        {
            tab.button.interactable = !isActive;
        }
        
        /// <summary>
        /// Helper method to load a given scene
        /// </summary>
        /// <param name="sceneName">Scene to load</param>
        private void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}
