using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Spawner for the coffee cups
    /// </summary>
    public class CoffeeCupSpawner : MonoBehaviour
    {
        [Tooltip("Coffee cup prefab")]
        [SerializeField] private GameObject coffeeCupPrefab;
        
        [Tooltip("Minimal number of coffee cups to spawn")]
        [SerializeField] private int minCoffeeCups = 1;
        
        [Tooltip("Maximal number of coffee cups to spawn")]
        [SerializeField] private int maxCoffeeCups = 5;
        
        [Tooltip("Minimal distance between coffee cups")]
        [SerializeField] private float minYSeparation = 2f;

        void Start()
        {
            // Apply the difficulty coefficients
            minCoffeeCups = (int)(minCoffeeCups * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxCoffeeCups = (int)(maxCoffeeCups * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            SpawnCoffeeCups();
        }
        
        /// <summary>
        /// Method for spawning the coffee cups
        /// </summary>
        void SpawnCoffeeCups()
        {
            int coffeeCupCount = Random.Range(minCoffeeCups, maxCoffeeCups + 1);
            
            // Get info about the hallway
            GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
            if (hallwayGenerator == null)
            {
                Debug.LogError("GenerateHallway component not found on the hallway GameObject.");
                return;
            }

            float hallwayWidth = hallwayGenerator.HallwayWidth;
            float hallwayLength = hallwayGenerator.HallwayLength;

            List<Vector2> spawnedPositions = new List<Vector2>();

            int attempts = 0;
            int maxAttempts = 100;
            
            // Attempt to spawn the cups  in a loop
            while (spawnedPositions.Count < coffeeCupCount && attempts < maxAttempts)
            {
                attempts++;
                // Generate new position
                float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
                float yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
                Vector2 spawnPosition = new Vector2(xPosition, yPosition);

                bool tooClose = false;
                foreach (Vector2 existingPosition in spawnedPositions)
                {
                    if (Vector2.Distance(spawnPosition, existingPosition) < minYSeparation)
                    {
                        tooClose = true;
                        break;
                    }
                }
                
                // If cups arent to close instantiate a new cup
                if (!tooClose)
                {
                    GameObject newCoffeeCup = Instantiate(coffeeCupPrefab, transform);
                    newCoffeeCup.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Reached maximum attempts while spawning coffee cups. Some coffee cups may not have been spawned.");
            }
        }
    }
}
