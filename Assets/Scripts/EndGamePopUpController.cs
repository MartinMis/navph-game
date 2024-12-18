using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Assets.Scripts.Utility;
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
        RunTimer.Instance.Disabled = false;
        CoinManager.Instance.FinalizeRunEarnings();
        SceneManager.LoadScene("HomeScene");
    }

    public void ContinueGame()
    {
        ToggleVisibility();
        SceneManager.LoadScene("GameScene");
        RunTimer.Instance.Disabled = true;
        GameObject.FindWithTag(Tags.Player).transform.position = new Vector3(0, -4, 0);
        _finalBoss = GameObject.FindWithTag("Boss");
    }
}
