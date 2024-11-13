using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogController : MonoBehaviour
{
    private float speed = 2f;
    private bool isMoving = true;
    private bool movingRight = true;

    private float minX;
    private float maxX;

    [SerializeField] private GameObject dogSoundPrefab;

    private GameObject dogSoundInstance;

    void Start()
    {
        if (dogSoundPrefab == null)
        {
            Debug.LogError("DogSound prefab is not assigned in the inspector!");
        }
    }

    public void SetDirection(bool right)
    {
        movingRight = right;
    }

    public void SetMovementLimits(float minXLimit, float maxXLimit)
    {
        minX = minXLimit;
        maxX = maxXLimit;
    }

    public bool IsMovingCheck()
    {
        return isMoving;
    }

    void Update()
    {
        if (isMoving)
        {
            float moveDirection = movingRight ? 1 : -1;
            transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);

            if (movingRight && transform.localPosition.x > maxX)
            {
                movingRight = false;
            }
            else if (!movingRight && transform.localPosition.x < minX)
            {
                movingRight = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Dog collided with: " + collision.gameObject.name);

        if (collision.CompareTag("Player"))
        {
            isMoving = false;
            //Debug.Log("ISMOVING SWITCH TO FALSE");
            //Debug.Log(isMoving);

            if (dogSoundPrefab != null)
            {
                dogSoundInstance = Instantiate(dogSoundPrefab, transform.position, Quaternion.identity);
                //Debug.Log("DogSound instantiated and played.");
            }
            else
            {
                Debug.LogError("DogSound prefab is not assigned!");
            }
        }
    }
}
