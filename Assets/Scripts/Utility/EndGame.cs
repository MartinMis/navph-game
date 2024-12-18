using Utility;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class EndGame
    {
        public static void ResetStatsAndEnd()
        {
            Object.Destroy(UIManager.Instance.gameObject);
            RunTimer.Instance.Disabled = false;
            CoinManager.Instance.FinalizeRunEarnings();
            SceneManager.LoadScene("HomeScene");
        }
    }
}