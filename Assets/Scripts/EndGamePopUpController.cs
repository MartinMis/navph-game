using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGamePopUpController : MonoBehaviour
{
    [SerializeField] GameObject uiElements;
    private GameObject _finalBoss;
    void Awake()
    {
        _finalBoss = GameObject.FindWithTag("Boss");
        _finalBoss.GetComponent<Boss>().OnDeath += ToggleVisibility;
    }

    void ToggleVisibility()
    {
        uiElements.SetActive(!uiElements.activeSelf);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("HomeScene");
    }

    public void ContinueGame()
    {
        ToggleVisibility();
        SceneManager.LoadScene("GameScene");
        _finalBoss = GameObject.FindWithTag("Boss");
    }
}
