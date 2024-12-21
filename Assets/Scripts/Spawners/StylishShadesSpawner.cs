using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Spawner for the stylish shades
    /// </summary>
    public class StylishShadesSpawner : MonoBehaviour
    {
        [Tooltip("Prefab used to spawn stylish shades.")]
        [SerializeField] private GameObject stylishShadesPrefab;
        
        [Tooltip("Minimal number of stylish shades to spawn")]
        [SerializeField] private int minStylishShades = 1;
        
        [Tooltip("Maximal number of stylish shades to spawn")]
        [SerializeField] private int maxStylishShades = 5;
        
        [Tooltip("Minimal distance between stylish shades")]
        [SerializeField] private float minSeparation = 2f;

        void Start()
        {
            // Adjust for the difficulty
            minStylishShades = (int)(minStylishShades * DifficultyManager.Instance.HallwayLengthCoeficient);
            maxStylishShades = (int)(maxStylishShades * DifficultyManager.Instance.HallwayLengthCoeficient);
            SpawnStylishShades();
        }

        void SpawnStylishShades()
        {
            int shadesCount = Random.Range(minStylishShades, maxStylishShades + 1);
            
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
            
            // Attempt to find a valid spawn position
            while (spawnedPositions.Count < shadesCount && attempts < maxAttempts)
            {
                attempts++;

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
                
                // If valid position was found spawn the shades
                if (!tooClose)
                {
                    GameObject newStylishShades = Instantiate(stylishShadesPrefab, transform);
                    newStylishShades.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Reached maximum attempts while spawning Stylish Shades. Some Stylish Shades may not have been spawned.");
            }
        }
    }
}
