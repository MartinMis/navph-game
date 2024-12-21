using System.Collections.Generic;
using Enemies;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Tea candle spawner
    /// </summary>
    public class TeaCandleSpawnerScript : MonoBehaviour
    {
        [Tooltip("Tea Candle prefab")]
        [SerializeField] private GameObject teaCandlePrefab;
        
        [Tooltip("Minimal number of tea candles to spawn")]
        [SerializeField] private int minCandleCount = 3;
        
        [Tooltip("Maximal number of tea candles to spawn")]
        [SerializeField] private int maxCandleCount = 7;
        
        [Tooltip("Minimal distance between the tea candles")]
        [SerializeField] private float minYSeparation = 5f;

        private List<Vector3> _spawnedPositions = new ();

        void Start()
        {
            // Adjust for the difficulty
            minCandleCount = (int)(minCandleCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxCandleCount = (int)(maxCandleCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            CreateTeaCandles();
        }

        void CreateTeaCandles()
        {
            int candleCount = Random.Range(minCandleCount, maxCandleCount + 1);
            // Get hallway size
            GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
            float hallwayWidth = hallwayGenerator.HallwayWidth;
            float hallwayLength = hallwayGenerator.HallwayLength;

            int attempts = 0;
            int maxAttempts = candleCount * 10;
            // Attempt to find a valid spawn position
            while (_spawnedPositions.Count < candleCount && attempts < maxAttempts)
            {
                attempts++;

                float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
                float yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
                Vector3 candlePosition = new Vector3(xPosition, yPosition, 0);

                bool tooClose = false;
                foreach (Vector3 pos in _spawnedPositions)
                {
                    if (Vector3.Distance(candlePosition, pos) < minYSeparation)
                    {
                        tooClose = true;
                        break;
                    }
                }

                float movementRadius = 2f;
                // Verify the candle will spawn within the bounds, if so instantiate it
                if (IsWithinBounds(candlePosition, movementRadius, hallwayWidth, hallwayLength) && !tooClose)
                {
                    GameObject newCandle = Instantiate(teaCandlePrefab, transform);
                    Debug.Log($"Spawning candle {candlePosition}");
                    newCandle.transform.localPosition = candlePosition;

                    TeaCandleController candleController = newCandle.GetComponent<TeaCandleController>();
                    if (candleController != null)
                    {
                        candleController.SetCenterPosition(candlePosition);
                    }

                    _spawnedPositions.Add(candlePosition);
                }
            }

            if (_spawnedPositions.Count < candleCount)
            {
                Debug.LogWarning("Could not place all tea candles with the given constraints.");
            }
        }
        
        /// <summary>
        /// Helper method for validating whether the candle will spawn in bounds
        /// </summary>
        /// <param name="position">Candle spawn position</param>
        /// <param name="radius">Radius in which the candle will move</param>
        /// <param name="width">Width of the hallway</param>
        /// <param name="length">Lenght of the hallway</param>
        /// <returns></returns>
        bool IsWithinBounds(Vector3 position, float radius, float width, float length)
        {
            float halfWidth = width / 2;
            float halfLength = length / 2;

            if (position.x - radius < -halfWidth || position.x + radius > halfWidth)
                return false;

            if (position.y - radius < -halfLength || position.y + radius > halfLength)
                return false;

            return true;
        }
    }
}
