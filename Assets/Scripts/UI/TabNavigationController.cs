using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private void UpdateButtonState(Tab tab, bool isActive)
    {
        UpdateButtonVisuals(tab, isActive);
        UpdateButtonInteractivity(tab, isActive);
    }

    private void UpdateButtonVisuals(Tab tab, bool isActive)
    {
        // swap sprite
        Image buttonImage = tab.button.GetComponent<Image>();
        buttonImage.sprite = isActive ? tab.config.activeSprite : tab.config.normalSprite;
    }

    private void UpdateButtonInteractivity(Tab tab, bool isActive)
    {
        tab.button.interactable = !isActive;
    }

    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
