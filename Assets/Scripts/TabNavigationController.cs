using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TabNavigationController : MonoBehaviour
{
    public Button upgradesButton;
    public Button homeButton;
    public Button collectionButton;
    public Button rankingButton;

    void Start()
    {
        // Získaj názov aktuálnej scény
        string currentScene = SceneManager.GetActiveScene().name;

        // Reset všetkých tlaèidiel na štandardný vzh¾ad
        ResetButtonStates();

        // Nastavenie aktívneho tlaèidla pod¾a názvu scény
        switch (currentScene)
        {
            case "UpgradesScene":
                SetActiveButton(upgradesButton);
                break;
            case "HomeScene":
                SetActiveButton(homeButton);
                break;
            case "CollectionScene":
                SetActiveButton(collectionButton);
                break;
            case "RankingScene":
                SetActiveButton(rankingButton);
                break;
        }

        upgradesButton.onClick.AddListener(LoadUpgradesScene);
        homeButton.onClick.AddListener(LoadHomeScene);
        collectionButton.onClick.AddListener(LoadCollectionScene);
        rankingButton.onClick.AddListener(LoadRankingScene);

    }

    void ResetButtonStates()
    {
        //reset all buttons to interactable
        upgradesButton.interactable = true;
        homeButton.interactable = true;
        collectionButton.interactable = true;
        rankingButton.interactable = true;
    }

    void SetActiveButton(Button activeButton)
    {
        // turn off interactable for active button
        activeButton.interactable = false;
        // change color later
    }

    public void LoadHomeScene()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void LoadUpgradesScene()
    {
        SceneManager.LoadScene("UpgradesScene");
    }

    public void LoadCollectionScene()
    {
        SceneManager.LoadScene("CollectionScene");
    }

    public void LoadRankingScene()
    {
        SceneManager.LoadScene("RankingScene");
    }
}