using Gameplay;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utility;

namespace Light
{
    /// <summary>
    /// Controller for the roof lights
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class RoofLightController : MonoBehaviour
    {
        [Tooltip("Radius of the roof light")]
        [SerializeField] private float radius;
        
        [Tooltip("Damage dealt by the light")]
        [SerializeField] private float damage;
    
        private bool _dealDamage;
        private PlayerController _playerController;
        
        void Start()
        {
            _playerController = GameObject.FindGameObjectWithTag(Tags.Player).GetComponent<PlayerController>();
            ModifyRadius(radius);
        }

        void Update()
        {
            // If player is standing in the light deal damage to them
            if (_dealDamage)
            {
                _playerController.DamagePlayer(damage*Time.deltaTime, DamageType.Light);
            }
        }
        
        // Method so the changes in radius are shown in Unity editor
        void OnValidate()
        {
            ModifyRadius(radius);
        }
        
        // When player enters start dealing damage to them
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _dealDamage = true;
            }
        }
        
        // When player leaves the light stop dealing damage to them
        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                _dealDamage = false;
            }
        }
        
        /// <summary>
        /// Method for modifying the radius of the roof lights
        /// </summary>
        /// <param name="newRadius">New radius to set</param>
        public void ModifyRadius(float newRadius)
        {
            radius = newRadius;
            GetComponent<CircleCollider2D>().radius = newRadius;
            GetComponent<Light2D>().pointLightOuterRadius = newRadius;
        }
    }
}
