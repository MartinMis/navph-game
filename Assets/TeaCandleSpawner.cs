using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeaCandleSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject teaCandlePrefab;
    [SerializeField] private int minCandleCount = 3;
    [SerializeField] private int maxCandleCount = 7;
    [SerializeField] private float candleMinDistance = 5f; // Minimum distance between candles

    private List<Vector3> spawnedPositions = new List<Vector3>();

    void Start()
    {
        CreateTeaCandles();
    }

    void CreateTeaCandles()
    {
        int candleCount = Random.Range(minCandleCount, maxCandleCount + 1);

        GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
        float hallwayWidth = hallwayGenerator.HallwayWidth;
        float hallwayLength = hallwayGenerator.HallwayLength;

        int attempts = 0;
        int maxAttempts = candleCount * 10; // Prevent infinite loops
        while (spawnedPositions.Count < candleCount && attempts < maxAttempts)
        {
            attempts++;

            // Random position within the hallway
            float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
            float yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
            Vector3 candlePosition = new Vector3(xPosition, yPosition, 0);

            // Check minimum distance from other candles
            bool tooClose = false;
            foreach (Vector3 pos in spawnedPositions)
            {
                if (Vector3.Distance(candlePosition, pos) < candleMinDistance)
                {
                    tooClose = true;
                    break;
                }
            }

            // Check if the candle's movement will stay within the hallway bounds
            float movementRadius = 2f; // The radius of the candle's circular movement
            if (IsWithinBounds(candlePosition, movementRadius, hallwayWidth, hallwayLength) && !tooClose)
            {
                GameObject newCandle = Instantiate(teaCandlePrefab, transform);
                newCandle.transform.localPosition = candlePosition;

                // Optionally set the center position in TeaCandleController
                TeaCandleController candleController = newCandle.GetComponent<TeaCandleController>();
                if (candleController != null)
                {
                    candleController.SetCenterPosition(candlePosition);
                }

                spawnedPositions.Add(candlePosition);
            }
        }

        if (spawnedPositions.Count < candleCount)
        {
            Debug.LogWarning("Could not place all tea candles with the given constraints.");
        }
    }

    bool IsWithinBounds(Vector3 position, float radius, float width, float length)
    {
        float halfWidth = width / 2;
        float halfLength = length / 2;

        // Check left and right bounds
        if (position.x - radius < -halfWidth || position.x + radius > halfWidth)
            return false;

        // Check top and bottom bounds
        if (position.y - radius < -halfLength || position.y + radius > halfLength)
            return false;

        return true;
    }
}
