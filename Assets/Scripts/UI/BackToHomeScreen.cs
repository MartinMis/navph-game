using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class BackToHomeScreen : MonoBehaviour
    {
        public void BackToHome()
        {
            SceneManager.LoadScene("HomeScene");
        }
    }
}
