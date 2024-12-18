using Assets.Scripts.Utility;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utility
{
    public class EndGame
    {
        public static void ResetStatsAndEnd()
        {
            RunTimer.Instance.Disabled = false;
            CoinManager.Instance.FinalizeRunEarnings();
            SceneManager.LoadScene("HomeScene");
        }
    }
}