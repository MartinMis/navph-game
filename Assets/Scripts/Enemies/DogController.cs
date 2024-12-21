using Spawners;
using UnityEngine;
using Utility;

namespace Enemies
{
    /// <summary>
    /// Controller for the dog enemies.
    /// </summary>
    public class DogController : MonoBehaviour
    {
        [Tooltip("How fast should the dog move")]
        [SerializeField] private float speed = 2f;
        
        public bool IsMoving { get; private set; } = true;

        private bool _movingRight = true;
        private float _minX;
        private float _maxX;
        private float _hallwayWidth;
        private float _dogWidth;
        private SpriteRenderer _sr;
        private Animator _animator;

        void Start()
        {
            GameObject hallway = GameObject.FindGameObjectWithTag(Tags.Hallway);
            if (hallway != null)
            {
                GenerateHallway generateHallway = hallway.GetComponent<GenerateHallway>();
                _hallwayWidth = generateHallway.HallwayWidth;
            }
            else
            {
                Debug.LogError("Hallway not found!");
            }

            _sr = GetComponent<SpriteRenderer>();
            if (_sr != null)
            {
                _dogWidth = _sr.bounds.size.x;
            }
            else
            {
                Debug.LogError("Dog SpriteRenderer not found!");
            }

            _animator = GetComponent<Animator>();
            if (_animator == null)
            {
                Debug.LogError("Animator not found on Dog!");
            }

            _minX = -_hallwayWidth / 2;
            _maxX = _hallwayWidth / 2;

            UpdateSpriteDirection();
            UpdateAnimatorParameters();
        }
        
        /// <summary>
        /// Helper method to set the dogs direction.
        /// </summary>
        /// <param name="right">Direction of the dog</param>
        public void SetDirection(bool right)
        {
            _movingRight = right;
            UpdateSpriteDirection();
        }

        void Update()
        {
            if (IsMoving)
            {
                // Move the dog, if he hits the limits of the hallway change his direction
                float moveDirection = _movingRight ? 1 : -1;
                transform.Translate(Vector2.right * moveDirection * speed * Time.deltaTime);
            
                if (_movingRight && transform.position.x > _maxX)
                {
                    _movingRight = false;
                    UpdateSpriteDirection();
                }
                else if (!_movingRight && transform.position.x < _minX)
                {
                    _movingRight = true;
                    UpdateSpriteDirection();
                }
            }
        }
        
        // Switch dog direction on collision
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag(Tags.Player))
            {
                IsMoving = false;
                UpdateAnimatorParameters();
            }
            else
            {
                _movingRight = !_movingRight;
                UpdateSpriteDirection();
            }
        }
        
        /// <summary>
        /// Update the sprite so it reflects dogs movement direction
        /// </summary>
        private void UpdateSpriteDirection()
        {
            if (_sr != null)
            {
                _sr.flipX = !_movingRight;
            }
        }
        
        /// <summary>
        /// Updates the animator according to the movement
        /// </summary>
        private void UpdateAnimatorParameters()
        {
            if (_animator != null)
            {
                _animator.SetBool("IsMoving", IsMoving);
            }
        }
    }
}
