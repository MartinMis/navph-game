using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class SceneChanger : MonoBehaviour
    {
        public void LoadHelp()
        {
            SceneManager.LoadScene("Help");
        }

        public void LoadGame()
        {
            SceneManager.LoadScene("GameScene");
        }

        public void LoadCredits()
        {
            SceneManager.LoadScene("Credits");
        }
    }
}

