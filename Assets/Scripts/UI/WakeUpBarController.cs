using Controllers;
using UnityEngine;
using UnityEngine.UI;
using Utility;

namespace UI
{
    public class WakeUpBarController : MonoBehaviour
    {
        [SerializeField] private Text maxValue;
        [SerializeField] private Image fill;
    
        private PlayerController _playerController;
        void Start()
        {
            _playerController = GameObject.FindGameObjectWithTag(Tags.Player)?.GetComponent<PlayerController>();
            if (_playerController == null)
            {
                Debug.LogError("[WakeUpBarController] No player with controller found");
                return;
            }

            maxValue.text = _playerController.maxSleepMeter.ToString("000");
            _playerController.OnWakeUpMeterUpdated += FillBar;
            FillBar();
        }

        void FillBar()
        {
            if (_playerController == null) return;
            fill.fillAmount = _playerController.GetCurrentWakeupMeter()/_playerController.maxSleepMeter;
        }

        void OnDestroy()
        {
            if (_playerController == null) return;
            _playerController.OnWakeUpMeterUpdated -= FillBar;
        }
    }
}
