using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField] private float damageAmount = 1f;
    [SerializeField] private float lifetime = 5f;
    [SerializeField] private float speed = 5f;

    private Transform playerTransform;
    private Rigidbody2D rb;

    private void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        Destroy(gameObject, lifetime);

        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody2D component missing from fire prefab.");
        }
    }

    private void Update()
    {
        if (playerTransform != null)
        {
            Vector2 direction = (playerTransform.position - transform.position).normalized;
            if (rb != null)
            {
                rb.velocity = direction * speed;
            }
        }
        else
        {
            if (rb != null)
            {
                rb.velocity = Vector2.zero;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.DamagePlayer(damageAmount, DamageType.Fire);
            }
            Destroy(gameObject);
        }
        else if (collision.gameObject.name == "Square")
        {
            Destroy(gameObject);
        }
    }
}
