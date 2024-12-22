using System;
using Bosses;
using UnityEngine;
using Upgrades;
using Gameplay;
using Light;
using Managers;
using UnityEngine.Serialization;
using Utility;

namespace Gameplay
{
    /// <summary>
    /// Singleton class for managing the sunrise timer
    /// </summary>
    public class RunTimer : MonoBehaviour
    {
        [Tooltip("How often should the timer update")]
        [SerializeField] private float updateInterval;
        
        [Tooltip("Maximal lenght of a run without upgrades")]
        [SerializeField] private float maxTime;
        
        [Tooltip("Maximal intenstity of light permisible")]
        [SerializeField] private float maxLightIntensity;
        
        public static RunTimer Instance;
        public bool disabled;
        
        private float _timer;
        private float _initialIntensity;
        
        /// <summary>
        /// Action invoked whenever the sunrise timer updates
        /// </summary>
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
            // Subscribe to current bosses on death event
            var boss = GameObject.FindGameObjectWithTag(Tags.Boss).GetComponent<LampBossController>();
            boss.OnDeath += DisableTimer;
            
            // Get the sunrise time upgrade and apply it
            var sunriseTimerUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Mask);
            sunriseTimerUpgrade?.ApplyEffect();
            if (sunriseTimerUpgrade is SunriseTimerUpgrade stu)
            {
                maxTime *= stu.SunriseTimerModifier;
            }
            // Get the current global light intensity
            _initialIntensity = GameObject.FindGameObjectWithTag(Tags.GlobalLight).GetComponent<SunriseController>().LightLevel;
        }
        
        void Update()
        {
            if (disabled) return;
            if (_timer > updateInterval)
            {
                _timer = 0;
                // Calculate new light level by multiplying the distance between max and initial intensity by fraction
                // of current time against max time allowed
                float newLightLevel = Mathf.Clamp(Time.timeSinceLevelLoad / maxTime * (maxLightIntensity - _initialIntensity) + _initialIntensity, 0f, maxLightIntensity);
                // Update the intensity
                var sunriseController = GameObject.FindGameObjectWithTag(Tags.GlobalLight)?.GetComponent<SunriseController>();
                if (sunriseController == null) return;
                sunriseController.LightLevel = newLightLevel;
                OnUpdate?.Invoke((newLightLevel-_initialIntensity)/(maxLightIntensity-_initialIntensity));
                
                // If intensity reaches max deal damage to player
                if (newLightLevel >= maxLightIntensity)
                {
                    var player = GameObject.FindGameObjectWithTag(Tags.Player);
                    player?.GetComponent<PlayerController>()?.DamagePlayer(1000);
                }
            }
            _timer += Time.deltaTime;
        
        }
        
        /// <summary>
        /// Method for disabling the sunrise timer
        /// </summary>
        void DisableTimer()
        {
            disabled = true;
        }
        
    }
}
