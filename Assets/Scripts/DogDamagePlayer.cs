using UnityEngine;

public class DogDamage : MonoBehaviour
{
    [SerializeField] private float damage = 0.1f;
    [SerializeField] private float damageRadius = 4f;
    private Transform playerTransform;
    private DogController dogController;

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
    }

    void Update()
    {
        if (playerTransform == null) return;

        if (!dogController.IsMovingCheck())
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
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw the damage radius in the editor for visualization
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, damageRadius);
    }
}
