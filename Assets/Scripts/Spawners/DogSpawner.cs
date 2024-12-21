using System.Collections;
using System.Collections.Generic;
using Enemies;
using Managers;
using UnityEngine;
using Utility;

namespace Spawners
{
    /// <summary>
    /// Spawner for dogs
    /// </summary>
    public class DogSpawner : MonoBehaviour
    {
        [Tooltip("Dog prefab")]
        [SerializeField] private GameObject dogPrefab;
        
        [Tooltip("Minimal number of dogs to spawn")]
        [SerializeField] private int minDogCount = 5;
        
        [Tooltip("Maximal number of dogs to spawn")]
        [SerializeField] private int maxDogCount = 15;
        
        [Tooltip("Minimal distance between the dogs")]
        [SerializeField] private float minYSeparation = 1f;
        
        private List<float> _spawnedDogYPositions = new ();
        private List<GameObject> _spawnedDogs = new ();
        
        void Start()
        {
            // Adjust spawn rate with difficulty coefficients
            minDogCount = (int)(minDogCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxDogCount = (int)(maxDogCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            CreateDogs();
        }
        
        /// <summary>
        /// Method for spawning the dogs
        /// </summary>
        void CreateDogs()
        {
            int dogCount = Random.Range(minDogCount, maxDogCount);
            // Get hallway size
            GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
            float hallwayWidth = hallwayGenerator.HallwayWidth;
            float hallwayLength = hallwayGenerator.HallwayLength;

            List<float> spawnedDogYPositions = new List<float>();
            
            // Generate dogs one by one
            for (int i = 0; i < dogCount; i++)
            {
                float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
                float yPosition = 0f;
                bool validPositionFound = false;
                int maxAttempts = 10;
                int attempt = 0;
                
                // Try to find a valid position for the dogs
                while (!validPositionFound && attempt < maxAttempts)
                {
                    yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
                    validPositionFound = true;

                    foreach (float existingY in spawnedDogYPositions)
                    {
                        if (Mathf.Abs(yPosition - existingY) < minYSeparation)
                        {
                            validPositionFound = false;
                            break;
                        }
                    }
                    attempt++;
                    
                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + new Vector3(xPosition, yPosition, 0), 15);
                    foreach (Collider2D collider in colliders)
                    {
                        Debug.Log($"[DogSpawner] {collider.gameObject.name}");
                        if (!collider.gameObject.CompareTag(Tags.Background))
                        {
                            validPositionFound = false;
                            break;
                        }
                    }
                }
                
                // If you find valid position instantiate a new dog 
                if (validPositionFound)
                {
                    Vector3 dogPosition = new Vector3(xPosition, yPosition, 0);

                    GameObject newDog = Instantiate(dogPrefab, transform);
                    newDog.transform.localPosition = dogPosition;

                    DogController dogController = newDog.GetComponent<DogController>();
                    if (dogController != null)
                    {
                        bool movingRight = Random.Range(0, 2) == 0;
                        dogController.SetDirection(movingRight);
                    }

                    spawnedDogYPositions.Add(yPosition);
                    _spawnedDogs.Add(newDog);
                }
                else
                {
                    Debug.LogWarning($"Could not find a valid position for dog {i + 1} after {maxAttempts} attempts.");
                }
            }
            // Delete any dogs that dont have enough space
            StartCoroutine(RemoveOverlapping());
        }
        
        /// <summary>
        /// Coroutine for deleting the overlapping dogs. Inspired by
        /// https://discussions.unity.com/t/how-to-wait-a-certain-amount-of-seconds-in-c/192244
        /// </summary>
        /// <remarks>
        /// Future implementation should use a more robust solution for overlapping
        /// </remarks>
        private IEnumerator RemoveOverlapping()
        {
            yield return new WaitForFixedUpdate();
            foreach (var dog in _spawnedDogs)
            {
                var spawnPosition = dog.transform.localPosition;
                var circleCenter = new Vector2(transform.position.x + spawnPosition.x, transform.position.y + spawnPosition.y);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCenter, 0.5f);
                foreach (Collider2D collider in colliders)
                {
                    if (!collider.gameObject.CompareTag(Tags.Background) && collider.gameObject != dog.gameObject)
                    {
                        Destroy(dog);
                        break;
                    }
                }
            }
        }
    }
}
