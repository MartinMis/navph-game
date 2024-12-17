using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class RunTimer : MonoBehaviour
{
    [SerializeField] private float updateInterval;
    [SerializeField] private float maxTime;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] SunriseController sunriseController;
    private float _timer;
    private float _initialIntensity;
    
    public event Action<float> OnUpdate; 
    void Start()
    {
        var sunriseTimerUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Mask);
        sunriseTimerUpgrade?.ApplyEffect();
        if (sunriseTimerUpgrade is SunriseTimerUpgrade stu)
        {
            maxTime *= stu.SunriseTimerModifier;
            Debug.Log($"Max time: {maxTime}");
        }
        _initialIntensity = sunriseController.LightLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (_timer > updateInterval)
        {
            _timer = 0;
            float newLightLevel = Mathf.Clamp(Time.timeSinceLevelLoad / maxTime * (maxLightIntensity - _initialIntensity) + _initialIntensity, 0f, maxLightIntensity);
            sunriseController.LightLevel = newLightLevel;
            OnUpdate?.Invoke((newLightLevel-_initialIntensity)/(maxLightIntensity-_initialIntensity));
        }
        _timer += Time.deltaTime;
    }
}
