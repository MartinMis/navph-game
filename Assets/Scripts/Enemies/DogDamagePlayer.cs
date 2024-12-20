using Controllers;
using UnityEngine;

namespace Enemies
{
    public class DogDamage : MonoBehaviour
    {
        [SerializeField] private float damage = 0.1f;
        [SerializeField] private float damageRadius = 4f;
        private Transform playerTransform;
        private DogController dogController;

        private AudioSource audioSource;

        void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogError("Player not found! Please ensure the player has the tag 'Player'.");
            }

            dogController = GetComponent<DogController>();
            if (dogController == null)
            {
                Debug.LogError("DogController script not found on the dog GameObject.");
            }

            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing on DogDamage.");
            }
        }

        void Update()
        {
            if (playerTransform == null) return;

            if (!dogController.IsMoving)
            {
                float distance = Vector2.Distance(transform.position, playerTransform.position);
                if (distance <= damageRadius)
                {
                    PlayerController playerController = playerTransform.GetComponent<PlayerController>();
                    if (playerController != null)
                    {
                        playerController.DamagePlayer(damage * Time.deltaTime);
                    }
                    else
                    {
                        Debug.LogError("PlayerController script not found on the player.");
                    }

                    if (audioSource != null)
                    {
                        float volume = Mathf.Clamp01(1 - (distance / damageRadius));
                        audioSource.volume = volume;

                        if (!audioSource.isPlaying)
                        {
                            audioSource.Play();
                        }
                    }
                }
                else
                {
                    if (audioSource != null && audioSource.isPlaying)
                    {
                        audioSource.Stop();
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
