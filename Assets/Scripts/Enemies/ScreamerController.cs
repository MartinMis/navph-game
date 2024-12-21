using Gameplay;
using UnityEngine;
using Utility;

namespace Enemies
{
    /// <summary>
    /// Class controlling the behavior of the screamer.
    /// </summary>
    public class ScreamerController : MonoBehaviour
    {
        [Tooltip("Maximal amount of damage screamer should deal")]
        [SerializeField] private float maxDamage = 1000f;
        
        [Tooltip("Maximal distance at which the screamer can attack")]
        [SerializeField] private float maxDistance = 20;
        
        [Tooltip("How much does the damage scale with distance")]
        [SerializeField] private float damageExponent = 2f;
        
        [Tooltip("How fast should the screamer move")]
        [SerializeField] private float movementSpeed = 2.5f;
        
        private AudioSource _audioSource;
        private Transform _playerTransform;
        

        void Awake()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
                if (_audioSource == null)
                {
                    Debug.LogError("[ScreamerController] AudioSource component missing on Screamer.");
                }
            }
        }

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("[ScreamerController] Player not found! Ensure the player has the 'Player' tag.");
            }
        }

        void Update()
        {
            if (_playerTransform == null)
                return;
            
            // Move in players direction
            Vector3 direction = (_playerTransform.position - transform.position).normalized;
            transform.position += direction * (movementSpeed * Time.deltaTime);
            float distance = Vector2.Distance(transform.position, _playerTransform.position);
            
            // If player is close enough
            if (distance <= maxDistance)
            {
                // Play the screaming sound
                float volume = Mathf.Clamp01(1 - (distance / maxDistance));
                if (_audioSource != null)
                {
                    _audioSource.volume = volume;

                    if (!_audioSource.isPlaying)
                    {
                        _audioSource.Play();
                    }
                }
                
                // Calculate the damage and damge the player
                float normalizedDistance = distance / maxDistance;
                float damageMultiplier = Mathf.Pow(1 - normalizedDistance, damageExponent);
                float damage = maxDamage * damageMultiplier;

                PlayerController playerController = _playerTransform.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DamagePlayer(damage*Time.deltaTime, DamageType.Sound);
                }
                else
                {
                    Debug.LogError("[ScreamerController]PlayerController component missing on Player.");
                }
            }
            else
            {
                if (_audioSource != null && _audioSource.isPlaying)
                {
                    _audioSource.Stop();
                }
            }
        }

        void OnDrawGizmosSelected()
        {
            // Draw a yellow sphere at the Screamer's position to represent max distance
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, maxDistance);
        }
    }
}