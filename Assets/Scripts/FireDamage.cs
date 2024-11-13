using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float lifetime = 5f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DamagePlayer(damageAmount);
            }
            else
            {
                Debug.LogError("PlayerController script not found on the player.");
            }

            Destroy(gameObject);
        }
    }
}
