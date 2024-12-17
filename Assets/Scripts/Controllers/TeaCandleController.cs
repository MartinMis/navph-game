using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaCandleController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float radius = 2f;
    [SerializeField] private float angularSpeed = 50f;

    [Header("Fire Settings")]
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private float fireInterval = 3f;
    [SerializeField] private int maxFires = 10;
    [SerializeField] private float fireRange = 10f;

    private int firesShot = 0;
    private float fireTimer = 0f;
    private Vector3 centerPosition;
    private float angle = 0f;

    private Transform playerTransform;

    public void SetCenterPosition(Vector3 center)
    {
        centerPosition = center;
    }

    void Start()
    {
        if (centerPosition == Vector3.zero)
            centerPosition = transform.position;

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        else
        {
            Debug.LogError("Player not found! Please ensure the player has the tag 'Player'.");
        }
    }

    void Update()
    {
        MoveInCircle();
        HandleShooting();
    }

    private void MoveInCircle()
    {
        angle += angularSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;

        Vector3 offset = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0) * radius;
        transform.localPosition = centerPosition + offset;
    }

    private void HandleShooting()
    {
        if (firesShot >= maxFires)
        {
            Destroy(gameObject);
            return;
        }

        if (playerTransform == null)
            return;

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        if (distanceToPlayer <= fireRange)
        {
            fireTimer += Time.deltaTime;
            if (fireTimer >= fireInterval)
            {
                ShootFire();
                fireTimer = 0f;
                firesShot++;
            }
        }
    }

    private void ShootFire()
    {
        Instantiate(firePrefab, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(centerPosition != Vector3.zero ? centerPosition : transform.position, radius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, fireRange);
    }
}
