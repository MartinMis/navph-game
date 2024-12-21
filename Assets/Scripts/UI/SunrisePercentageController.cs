using Bosses;
using Gameplay;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    /// <summary>
    /// Class for the sunrise percentage
    /// </summary>
    public class SunrisePercentageController : MonoBehaviour
    {
        [Tooltip("Text with sunrise progress")]
        [SerializeField] Text percentageText;

        void Awake()
        {
            // If the timer is disabled disable this aswell
            if (RunTimer.Instance.disabled)
            {
                Disable();
            }
            else
            {
                GameObject.FindWithTag(Tags.Boss).GetComponent<LampBossController>().OnDeath += Disable;
                RunTimer.Instance.OnUpdate += ChangePercentage;
            }
        }
        
        /// <summary>
        /// Method for updating the percentage
        /// </summary>
        /// <param name="percentage">New percentage</param>
        void ChangePercentage(float percentage)
        {
            Debug.Log($"Percentage: {percentage}");
            percentageText.text = Mathf.RoundToInt(percentage*100) + "%";
        }
        
        /// <summary>
        /// Method for disabling the sunrise percentage
        /// </summary>
        void Disable()
        {
            Destroy(gameObject);
        }

        void OnDestroy()
        {
            RunTimer.Instance.OnUpdate -= ChangePercentage;
        }
    }
}
