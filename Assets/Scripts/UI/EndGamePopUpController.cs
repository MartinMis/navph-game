using Bosses;
using Gameplay;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class EndGamePopUpController : MonoBehaviour
    {
        [SerializeField] GameObject uiElements;
        [SerializeField] private Text text;
        private GameObject _finalBoss;
        void Awake()
        {
            _finalBoss = GameObject.FindWithTag("Boss");
            _finalBoss.GetComponent<LampBossController>().OnVictory += ToggleVisibility;
        }

        void ToggleVisibility(int reward = 0)
        {
            text.text = $"Sunrise has been ended! You gain {reward} coins";
            uiElements.SetActive(!uiElements.activeSelf);
        }

        public void BackToMainMenu()
        {
            EndGame.ResetStatsAndEnd();
        }

        public void ContinueGame()
        {
            ToggleVisibility();
            SceneManager.LoadScene("GameScene");
            RunTimer.Instance.Disabled = true;
            GameObject.FindWithTag(Tags.Player).transform.position = new Vector3(0, -9, 0);
            _finalBoss = GameObject.FindWithTag("Boss");
        }
    }
}
