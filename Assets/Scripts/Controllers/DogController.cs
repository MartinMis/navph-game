using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    public bool IsMoving { get; private set; } = true;

    private bool movingRight = true;

    private float minX;
    private float maxX;

    [SerializeField] private GameObject dogSoundPrefab;

    private GameObject dogSoundInstance;

    private float hallwayWidth;
    private float dogWidth;

    private SpriteRenderer sr;

    void Awake()
    {
        if (dogSoundPrefab == null)
        {
            Debug.LogError("DogSound prefab is not assigned in the inspector!");
        }
    }

    void Start()
    {
        GameObject hallway = GameObject.FindObjectOfType<GenerateHallway>().gameObject;
        if (hallway != null)
        {
            GenerateHallway generateHallway = hallway.GetComponent<GenerateHallway>();
            hallwayWidth = generateHallway.HallwayWidth;
        }
        else
        {
            Debug.LogError("Hallway not found!");
        }

        sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            dogWidth = sr.bounds.size.x;
        }
        else
        {
            Debug.LogError("Dog SpriteRenderer not found!");
        }

        minX = -hallwayWidth / 2 + dogWidth / 2;
        maxX = hallwayWidth / 2 - dogWidth / 2;

        UpdateSpriteDirection();
    }

    public void SetDirection(bool right)
    {
        movingRight = right;
        UpdateSpriteDirection();
    }

    void Update()
    {
        if (IsMoving)
        {
            float moveDirection = movingRight ? 1 : -1;
            transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);
            
            /*
            if (movingRight && transform.position.x > maxX)
            {
                movingRight = false;
                UpdateSpriteDirection();
            }
            else if (!movingRight && transform.position.x < minX)
            {
                movingRight = true;
                UpdateSpriteDirection();
            }
            */
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            IsMoving = false;

            if (dogSoundPrefab != null)
            {
                dogSoundInstance = Instantiate(dogSoundPrefab, transform.position, Quaternion.identity);
            }
            else
            {
                Debug.LogError("DogSound prefab is not assigned!");
            }
        }
        else
        {
            movingRight = !movingRight;
            UpdateSpriteDirection();
        }
    }

    private void UpdateSpriteDirection()
    {
        if (sr != null)
        {
            sr.flipX = !movingRight;
        }
    }
}
