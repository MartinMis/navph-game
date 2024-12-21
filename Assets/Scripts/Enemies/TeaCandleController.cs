using UnityEngine;
using Utility;

namespace Enemies
{
    /// <summary>
    /// Controller for the tea candle enemy.
    /// </summary>
    public class TeaCandleController : MonoBehaviour
    {
        [Header("Movement Settings")]
        [Tooltip("Radius in which candle circles")]
        [SerializeField] private float radius = 2f;
        
        [Tooltip("Speed at which the candle circles")]
        [SerializeField] private float angularSpeed = 50f;

        [Header("Fire Settings")]
        [Tooltip("Prefab for the fire projectile")]
        [SerializeField] private GameObject firePrefab;
        
        [Tooltip("How often should the candle fire a projectile")]
        [SerializeField] private float fireInterval = 3f;
        
        [Tooltip("How many times should the candle fire a projectile")]
        [SerializeField] private int maxFires = 10;
        
        [Tooltip("At what range should the candle fire")]
        [SerializeField] private float fireRange = 10f;

        private int _firesShot;
        private float _fireTimer;
        private Vector3 _centerPosition;
        private float _angle;
        private Transform _playerTransform;

        /// <summary>
        /// Method for setting the center position around which the candle circles.
        /// </summary>
        /// <param name="center">Center position</param>
        public void SetCenterPosition(Vector3 center)
        {
            _centerPosition = center;
        }

        void Start()
        {
            if (_centerPosition == Vector3.zero)
                _centerPosition = transform.position;

            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("[TeaCandleController] Player not found! Please ensure the player has the tag 'Player'.");
            }
        }

        void Update()
        {
            MoveInCircle();
            HandleShooting();
        }

        /// <summary>
        /// Method for moving the tea candle in a circle using trigonometry
        /// </summary>
        private void MoveInCircle()
        {
            _angle += angularSpeed * Time.deltaTime;
            float rad = _angle * Mathf.Deg2Rad;

            Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
            transform.localPosition = _centerPosition + offset;
        }
        
        /// <summary>
        /// Method for handling shooting of projectiles
        /// </summary>
        private void HandleShooting()
        {
            // If all projectiles have been shot, destroy the tea candle
            if (_firesShot >= maxFires)
            {
                Destroy(gameObject);
                return;
            }

            if (_playerTransform == null)
                return;
            
            // Calculate the distance to the player and if its in range and enough time has passed shoot
            float distanceToPlayer = Vector2.Distance(transform.position, _playerTransform.position);
            if (distanceToPlayer <= fireRange)
            {
                _fireTimer += Time.deltaTime;
                if (_fireTimer >= fireInterval)
                {
                    ShootFire();
                    _fireTimer = 0f;
                    _firesShot++;
                }
            }
        }
        
        /// <summary>
        /// Method for spawning projectile instances.
        /// </summary>
        private void ShootFire()
        {
            Instantiate(firePrefab, transform.position, Quaternion.identity);
        }
        
        // Destroy tea candle when touched by the player
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                Destroy(gameObject);
            }
        }
    
    }
}
