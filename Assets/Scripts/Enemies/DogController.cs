using Spawners_and_Generators;
using UnityEngine;

namespace Enemies
{
    public class DogController : MonoBehaviour
    {
        [SerializeField] private float speed = 2f;
        public bool IsMoving { get; private set; } = true;

        private bool movingRight = true;

        private float minX;
        private float maxX;

        private float hallwayWidth;
        private float dogWidth;

        private SpriteRenderer sr;
        private Animator animator;

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

            animator = GetComponent<Animator>();
            if (animator == null)
            {
                Debug.LogError("Animator not found on Dog!");
            }

            minX = -hallwayWidth / 2;
            maxX = hallwayWidth / 2;

            UpdateSpriteDirection();
            UpdateAnimatorParameters();
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
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                IsMoving = false;
                UpdateAnimatorParameters();
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

        private void UpdateAnimatorParameters()
        {
            if (animator != null)
            {
                animator.SetBool("IsMoving", IsMoving);
            }
        }
    }
}
