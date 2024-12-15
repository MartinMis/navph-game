using System;
using UnityEngine;

public class ZZZPointController : MonoBehaviour
{
    public event Action<ZZZPointController> OnCollected;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Collect();
        }
    }

    private void Collect()
    {
        OnCollected?.Invoke(this);
        Destroy(gameObject);
    }
}
