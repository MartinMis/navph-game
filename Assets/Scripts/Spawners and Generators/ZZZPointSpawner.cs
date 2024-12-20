using System.Collections.Generic;
using UnityEngine;

public class ZZZPointSpawner : MonoBehaviour
{
    [Header("ZZZ Point Settings")]
    [SerializeField] private GameObject zzzPointPrefab;
    [SerializeField] private int minZZZPoints = 10;
    [SerializeField] private int maxZZZPoints = 20;
    [SerializeField] private float minSeparation = 2f;

    private void Start()
    {
        minZZZPoints = (int)(minZZZPoints * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        maxZZZPoints = (int)(maxZZZPoints * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        SpawnZZZPoints();
    }

    private void SpawnZZZPoints()
    {
        if (zzzPointPrefab == null)
        {
            Debug.LogError("ZZZPoint Prefab is not assigned in the inspector.");
            return;
        }

        int zzzPointCount = Random.Range(minZZZPoints, maxZZZPoints + 1);

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

            if (!tooClose)
            {
                GameObject newZZZPoint = Instantiate(zzzPointPrefab, transform);
                newZZZPoint.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                spawnedPositions.Add(spawnPosition);
            }
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Reached maximum attempts while spawning ZZZ Points. Some ZZZ Points may not have been spawned.");
        }
    }
}
