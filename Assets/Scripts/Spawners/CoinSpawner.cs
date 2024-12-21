using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Spawners
{
    /// <summary>
    /// Spawner for the coins
    /// </summary>
    public class CoinSpawner : MonoBehaviour
    {
        [Header("Coin Settings")]
        [Tooltip("Coin Prefab")]
        [SerializeField] private GameObject coinPrefab;
        
        [Tooltip("Minimal number of coins to spawn")]
        [SerializeField] private int minCoins = 10;
        
        [Tooltip("Maximal number of coins to spawn")]
        [SerializeField] private int maxCoins = 20;
        
        [Tooltip("Minimal distance between the coins")]
        [SerializeField] private float minSeparation = 2f;

        private void Start()
        {
            // Adjust for the difficulty
            minCoins = (int)(minCoins * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxCoins = (int)(maxCoins * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            SpawnCoins();
        }

        private void SpawnCoins()
        {
            if (coinPrefab == null)
            {
                Debug.LogError("ZZZPoint Prefab is not assigned in the inspector.");
                return;
            }

            int zzzPointCount = Random.Range(minCoins, maxCoins + 1);
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
            // Attempt to spawn coins
            while (spawnedPositions.Count < zzzPointCount && attempts < maxAttempts)
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
                
                // If valid position was found spawn coin
                if (!tooClose)
                {
                    GameObject newCoin = Instantiate(coinPrefab, transform);
                    newCoin.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Reached maximum attempts while spawning ZZZ coins. Some ZZZ coins may not have been spawned.");
            }
        }
    }
}
