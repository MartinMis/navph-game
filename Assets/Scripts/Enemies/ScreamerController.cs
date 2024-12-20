using UnityEngine;

public class ScreamerController : MonoBehaviour
{
    [SerializeField] private float maxDamage = 1000f;
    [SerializeField] private float maxDistance = 20;
    [SerializeField] private float damageExponent = 2f;
    [SerializeField] private float movementSpeed = 2.5f;
    [SerializeField] private AudioSource audioSource;

    private Transform playerTransform;

    private bool isAudioPlaying = false;

    void Awake()
    {
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("AudioSource component missing on Screamer.");
            }
        }
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Ensure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        Vector3 direction = (playerTransform.position - transform.position).normalized;

        transform.position += direction * movementSpeed * Time.deltaTime;

        float distance = Vector2.Distance(transform.position, playerTransform.position);

        if (distance <= maxDistance)
        {
            float volume = Mathf.Clamp01(1 - (distance / maxDistance));
            if (audioSource != null)
            {
                audioSource.volume = volume;

                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                    isAudioPlaying = true;
                }
            }

            float normalizedDistance = distance / maxDistance;
            float damageMultiplier = Mathf.Pow(1 - normalizedDistance, damageExponent);
            float damage = maxDamage * damageMultiplier;

            PlayerController playerController = playerTransform.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DamagePlayer(damage, DamageType.Sound);
            }
            else
            {
                Debug.LogError("PlayerController component missing on Player.");
            }
        }
        else
        {
            if (audioSource != null && audioSource.isPlaying)
            {
                audioSource.Stop();
                isAudioPlaying = false;
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