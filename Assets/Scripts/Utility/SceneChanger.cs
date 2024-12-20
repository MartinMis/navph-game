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
    }
}

