using Gameplay;
using UnityEngine;
using Utility;

namespace Enemies
{
    /// <summary>
    /// Component managing the dealing of damage to the player with fire
    /// </summary>
    public class FireDamage : MonoBehaviour
    {
        [Tooltip("How much damage to deal")]
        [SerializeField] private float damageAmount = 1f;
        
        [Tooltip("How long should the fire exist")]
        [SerializeField] private float lifetime = 5f;
        
        [Tooltip("How fast should the fire move")]
        [SerializeField] private float speed = 5f;

        private Transform _playerTransform;
        private Rigidbody2D _rb;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag(Tags.Player);
            if (player != null)
            {
                _playerTransform = player.transform;
            }

            Destroy(gameObject, lifetime);

            _rb = GetComponent<Rigidbody2D>();
            if (_rb == null)
            {
                Debug.LogError("[FireDamage] Rigidbody2D component missing from fire prefab.");
            }
        }

        private void Update()
        {
            // Calculate direction to the player and move in it
            if (_playerTransform != null)
            {
                Vector2 direction = (_playerTransform.position - transform.position).normalized;
                if (_rb != null)
                {
                    _rb.velocity = direction * speed;
                }
            }
            else
            {
                if (_rb != null)
                {
                    _rb.velocity = Vector2.zero;
                }
            }
        }
        
        // On collision either deal damage to the player or destroy the fire object
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag(Tags.Player))
            {
                PlayerController playerController = collision.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DamagePlayer(damageAmount, DamageType.Fire);
                }
                Destroy(gameObject);
            }
            else if (collision.CompareTag(Tags.Furniture))
            {
                Destroy(gameObject);
            }
        }
    }
}
