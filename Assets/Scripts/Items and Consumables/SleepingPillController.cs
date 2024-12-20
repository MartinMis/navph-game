using UnityEngine;

public class SleepingPillController: MonoBehaviour
{
    [SerializeField] private float healAmount = 10f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController playerController = collision.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.Heal(healAmount);
            }
            else
            {
                Debug.LogError("PlayerController script not found on the player.");
            }

            Destroy(gameObject);
        }
    }
}