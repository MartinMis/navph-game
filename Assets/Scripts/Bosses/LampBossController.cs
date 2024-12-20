using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utility;
using Triggers;
using Random = UnityEngine.Random;

namespace Bosses
{
    public class LampBossController : Boss
    {
        [Tooltip("Number of light rays Lamp should spawn")]
        [SerializeField] private int lightRayCount = 8;
        
        [Tooltip("Light ray prefab")]
        [SerializeField] private GameObject lightRayPrefab;
        
        [Tooltip("Trigger for when player enters the boss room")]
        [SerializeField] private PlayerEnterTrigger playerEnterTrigger;
        
        [Tooltip("How much should the lights dim")]
        [SerializeField] private float dimLightIntensity = 0.5f;
        
        [Tooltip("Combined angle of all the light rays next to each other")]
        [SerializeField] private float combinedLightAngle = 180.0f;
        
        [Tooltip("How much should the light drop off in each ray")]
        [SerializeField] private float innerLightDropOff = 0.9f;
        
        [Header("Initial attack settings")]
        [Tooltip("Safe time before the boss first attacks")]
        [SerializeField] float timeBeforeFirstAttack = 2f;
        
        [Tooltip("Time between individual attacks")]
        [SerializeField] float timeBetweenAttacks = 4f;
        
        [Tooltip("Time when the safe zone begins")]
        [SerializeField] float attackSafezoneTime = 2f;
        
        [Tooltip("Time when the boss starts rotating")]
        [SerializeField] float rotationWindowBegining = 1.5f;
        
        [Tooltip("Time when the boss stops rotating")]
        [SerializeField] float rotationWindowEnding = 1f;
        
        [Tooltip("Minimal rotation")]
        [SerializeField] float minRotation = -5f;
        
        [Tooltip("Maximal rotation")]
        [SerializeField] float maxRotation = 5f;
        
        private readonly List<GameObject> _lightRays = new ();
        private float _rayWidth;
        private float _timeRemaining = 2;
        private float _rotationTarget;
        private bool _playerInRoom;

        void Start()
        {
            CreateLightRays();
            ChangeLightStrength(dimLightIntensity, false);
            _timeRemaining = timeBetweenAttacks;
            playerEnterTrigger.OnTriggered += TriggeredCallback;
        }

        
        void Update()
        {
            // If player is not in the room don't do anything
            if (!_playerInRoom) return;
            if (_timeRemaining > 0)
            {
                _timeRemaining -= Time.deltaTime;
            }

            if (_timeRemaining < attackSafezoneTime)
            {
                ChangeLightStrength(dimLightIntensity, false);
            }

            if (_timeRemaining <= 1.5 && _timeRemaining > 1)
            {
                RotateLightRays(_rotationTarget);
            }

            if (_timeRemaining < 0)
            {
                _rotationTarget = Random.Range(-5f, 5f);
                ChangeLightStrength(1, true);
                _timeRemaining = timeBetweenAttacks;
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
        /// Function initializing light rays when the boss spawns.
        /// </summary>
        void CreateLightRays()
        {
            // Calculate how wide the ray should be. Rays can never fill more than half the room
            _rayWidth = combinedLightAngle/lightRayCount;
            var rayAngle = _rayWidth * 2;
            for (var i = 0; i < lightRayCount; i++)
            {
                // Instantiate new light ray and orient it correctly
                GameObject newLightRay = Instantiate(lightRayPrefab, transform);
                newLightRay.transform.localEulerAngles = new Vector3(0, 0, rayAngle*i);
                // Set Light2D component to output light in the calculated ray width
                Light2D newLightRayLight2D = newLightRay.GetComponent<Light2D>();
                newLightRayLight2D.pointLightOuterAngle = _rayWidth;
                newLightRayLight2D.pointLightInnerAngle = _rayWidth * innerLightDropOff;
                // Add the new ray to the list
                _lightRays.Add(newLightRay);
            }
        }
        
        /// <summary>
        /// Method for changing the intensity of all the light rays and turning on or off their damage component.
        /// </summary>
        /// <param name="intensity">Light intensity of the light rays</param>
        /// <param name="dealDamage">Turns the damage on and off</param>
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
        
        /// <summary>
        /// Method for rotating the light rays by the given angle
        /// </summary>
        /// <param name="angle">Angle to rotate light rays by</param>
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
