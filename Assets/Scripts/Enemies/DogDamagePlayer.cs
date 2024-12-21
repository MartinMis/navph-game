using Gameplay;
using UnityEngine;
using Utility;

namespace Enemies
{
    /// <summary>
    /// Component for damaging player by dog
    /// </summary>
    public class DogDamage : MonoBehaviour
    {
        [Tooltip("Damage amount")]
        [SerializeField] private float damage = 0.1f;
        
        [Tooltip("In what radius should the damage be dealt")]
        [SerializeField] private float damageRadius = 4f;
        
        private Transform _playerTransform;
        private DogController _dogController;
        private AudioSource _audioSource;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player != null)
            {
                _playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("[DogDamagePlayer] Player not found! Please ensure the player has the tag 'Player'.");
            }

            _dogController = GetComponent<DogController>();
            if (_dogController == null)
            {
                Debug.LogError("[DogDamagePlayer] DogController script not found on the dog GameObject.");
            }

            _audioSource = GetComponent<AudioSource>();
            if (_audioSource == null)
            {
                Debug.LogError("[DogDamagePlayer] AudioSource component missing on DogDamage.");
            }
        }

        void Update()
        {
            if (_playerTransform == null) return;

            if (!_dogController.IsMoving)
            {
                // If dog is stopped and player is in the range deal damage to him and play the barking sound effect
                float distance = Vector2.Distance(transform.position, _playerTransform.position);
                if (distance <= damageRadius)
                {
                    PlayerController playerController = _playerTransform.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.DamagePlayer(damage * Time.deltaTime);
                    }
                    else
                    {
                        Debug.LogError("[DogDamagePlayer] PlayerController script not found on the player.");
                    }

                    if (_audioSource != null)
                    {
                        float volume = Mathf.Clamp01(1 - (distance / damageRadius));
                        _audioSource.volume = volume;

                        if (!_audioSource.isPlaying)
                        {
                            _audioSource.Play();
                        }
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
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, damageRadius);
        }
    }
}
