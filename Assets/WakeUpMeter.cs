using System.Collections;
using System.Collections.Generic;
using Utility;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WakeUpMeter : MonoBehaviour
{
    PlayerController _playerController;
    
    void Start()
    {
        
        _playerController = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerController>();
        _playerController.OnWakeUpMeterUpdated += UpdateWakeUpMeter;
        UpdateWakeUpMeter();
    }

    void UpdateWakeUpMeter()
    {
        GetComponent<TextMeshProUGUI>().text = $"WakeUp Meter: " + GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerController>().GetCurrentWakeupMeter();
    }

    void OnDestroy()
    {
        _playerController.OnWakeUpMeterUpdated -= UpdateWakeUpMeter;
    }
}
