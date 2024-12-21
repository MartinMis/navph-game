using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Spawner for decaf coffee
    /// </summary>
    public class DecafCoffeeSpawner : MonoBehaviour
    {
        [Tooltip("Decaf coffee prefab")]
        [SerializeField] private GameObject decafCoffeePrefab;
        
        [Tooltip("Minimal number of coffees to spawn")]
        [SerializeField] private int minDecafCoffees = 1;
        
        [Tooltip("Maximal number of coffees to spawn")]
        [SerializeField] private int maxDecafCoffees = 5;
        
        [Tooltip("Minimal separation between the decaf coffees")]
        [SerializeField] private float minSeparation = 2f;

        void Start()
        {
            // Apply the difficulty coefficients
            minDecafCoffees = (int)(minDecafCoffees * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxDecafCoffees = (int)(maxDecafCoffees * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            SpawnDecafCoffees();
        }
        
        /// <summary>
        /// Method for spawning the decaf coffees
        /// </summary>
        void SpawnDecafCoffees()
        {
            int coffeeCount = Random.Range(minDecafCoffees, maxDecafCoffees + 1);
            
            // Get hallway size
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
            
            // Attempt to spawn new decaf coffees in a loop
            while (spawnedPositions.Count < coffeeCount && attempts < maxAttempts)
            {
                attempts++;
                
                // Generate a new position
                float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
                float yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
                Vector2 spawnPosition = new Vector2(xPosition, yPosition);

                bool tooClose = false;
                foreach (Vector2 existingPosition in spawnedPositions)
                {
                    if (Vector2.Distance(spawnPosition, existingPosition) < minSeparation)
                    {
                        tooClose = true;
                        break;
                    }
                }
                
                // If items are not too close spawn new decaf coffee
                if (!tooClose)
                {
                    GameObject newDecafCoffee = Instantiate(decafCoffeePrefab, transform);
                    newDecafCoffee.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Reached maximum attempts while spawning decaf coffees. Some decaf coffees may not have been spawned.");
            }
        }
    }
}
