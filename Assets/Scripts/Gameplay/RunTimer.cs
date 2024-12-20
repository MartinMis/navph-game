using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Utility;
using UnityEngine;
using Bosses;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.Rendering.Universal;
using UnityEngine.Serialization;

public class RunTimer : MonoBehaviour
{
    [SerializeField] private float updateInterval;
    [SerializeField] private float maxTime;
    [SerializeField] private float maxLightIntensity;
    public static RunTimer Instance;
    private float _timer;
    private float _initialIntensity;
    public bool Disabled = false;
    
    public event Action<float> OnUpdate;

    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this);
            Instance = this;
        }
        else if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        LampBossController boss = GameObject.FindGameObjectWithTag("Boss").GetComponent<LampBossController>();
        boss.OnDeath += DisableTimer;
        
        var sunriseTimerUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Mask);
        sunriseTimerUpgrade?.ApplyEffect();
        if (sunriseTimerUpgrade is SunriseTimerUpgrade stu)
        {
            maxTime *= stu.SunriseTimerModifier;
            Debug.Log($"Max time: {maxTime}");
        }
        _initialIntensity = GameObject.FindGameObjectWithTag("GlobalLight").GetComponent<SunriseController>().LightLevel;
    }

    // Update is called once per frame
    void Update()
    {
        if (Disabled) return;
        if (_timer > updateInterval)
        {
            _timer = 0;
            float newLightLevel = Mathf.Clamp(Time.timeSinceLevelLoad / maxTime * (maxLightIntensity - _initialIntensity) + _initialIntensity, 0f, maxLightIntensity);
            var sunriseController = GameObject.FindGameObjectWithTag("GlobalLight")?.GetComponent<SunriseController>();
            if (sunriseController == null) return;
            sunriseController.LightLevel = newLightLevel;
            OnUpdate?.Invoke((newLightLevel-_initialIntensity)/(maxLightIntensity-_initialIntensity));

            if (newLightLevel >= maxLightIntensity)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                player?.GetComponent<PlayerController>()?.DamagePlayer(1000);
            }
        }
        _timer += Time.deltaTime;
        
    }

    void DisableTimer()
    {
        Disabled = true;
    }
}
