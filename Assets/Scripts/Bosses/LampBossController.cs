using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utility;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class LampBossController : Boss
    {
        [Tooltip("Number of light rays Lamp should spawn")]
        [SerializeField] private int lightRayCount = 8;
        
        [Tooltip("Light ray prefab")]
        [SerializeField] private GameObject lightRayPrefab;
        [SerializeField] private PlayerEnterTrigger playerEnterTrigger;
        [SerializeField] float timeBeforeFirstAttack = 2f;
        
        [Header("Initial attack settings")]
        [SerializeField] float timeBetweenAttacks = 4f;
        [SerializeField] float attackSafezoneTime = 2f;
        [SerializeField] float rotationWindowBegining = 1.5f;
        [SerializeField] float rotationWindowEnding = 1f;
        [SerializeField] float minRotation = -5f;
        [SerializeField] float maxRotation = 5f;
        
        private float _rayWidth = 0;
        private List<GameObject> _lightRays = new List<GameObject>();

        
        private float _timeRemaining = 2;
        private float _dimLightIntensity = 0.5f;
        private float _rotationTarget = 0f;
        private bool _playerInRoom = false;

        void Start()
        {
            CreateLightRays();
            ChangeLightStrength(_dimLightIntensity, false);
            playerEnterTrigger.OnTriggered += TriggeredCallback;
        }

        
        void Update()
        {
            if (!_playerInRoom) return;
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }

            if (_timeRemaining < attackSafezoneTime)
            {
                ChangeLightStrength(_dimLightIntensity, false);
            }

            if (_timeRemaining <= 1.5 && _timeRemaining > 1)
            {
                RotateLightRays(_rotationTarget);
            }

            if (_timeRemaining < 0)
            {
                _rotationTarget = Random.Range(-5f, 5f);
                ChangeLightStrength(1, true);
                _timeRemaining = 4;
            }
        }

        void TriggeredCallback()
        {
            var waiter = new Waiter();
            StartCoroutine(waiter.WaitAndExecuteCoroutine(timeBeforeFirstAttack, StartFight));
        }
        
        void StartFight()
        {
            _playerInRoom = true;
        }
        
        /// <summary>
        /// Function initializing lightrays when the boss spawns
        /// </summary>
        void CreateLightRays()
        {
            _rayWidth = 180/lightRayCount;
            float rayAngle = _rayWidth * 2;
            for (int i = 0; i < lightRayCount; i++)
            {
                GameObject newLightRay = Instantiate(lightRayPrefab, transform);
                newLightRay.transform.localEulerAngles = new Vector3(0, 0, rayAngle*i);
                Light2D newLightRayLight2D = newLightRay.GetComponent<Light2D>();
                newLightRayLight2D.pointLightOuterAngle = _rayWidth;
                newLightRayLight2D.pointLightInnerAngle = _rayWidth * 0.9f;
                _lightRays.Add(newLightRay);
            }
        }

        void ChangeLightStrength(float intensity, bool dealDamage = true)
        {
            foreach (var lightRay in _lightRays)
            {
                var lightEmitter = lightRay.GetComponent<Light2D>();
                var lightDamage = lightRay.GetComponent<DamagePlayer>();
                lightEmitter.intensity = intensity;
                lightDamage.enabled = dealDamage; 
            }
        }

        void RotateLightRays(float angle)
        {
            foreach (var lightRay in _lightRays)
            {
                var currentRotation = lightRay.transform.localEulerAngles;
                lightRay.transform.localEulerAngles = currentRotation + new Vector3(0, 0, angle);
            }
        }
    }
}
