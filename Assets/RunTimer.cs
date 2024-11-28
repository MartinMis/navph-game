using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Controls;

public class RunTimer : MonoBehaviour
{
    [SerializeField] private float updateInterval;
    [SerializeField] private float maxTime;
    [SerializeField] private float maxLightIntensity;
    [SerializeField] SunriseController sunriseController;
    private float _timer;
    private float _initialIntensity;
    void Start()
    {
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
        }
        _timer += Time.deltaTime;
    }
}
