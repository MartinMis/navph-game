using System.Collections.Generic;
using UnityEngine;

public class StackOfPillsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject stackOfPillsPrefab;
    [SerializeField] private int minPillsStacks = 1;
    [SerializeField] private int maxPillsStacks = 5;
    [SerializeField] private float minYSeparation = 2f;

    void Start()
    {
        minPillsStacks = (int)(minPillsStacks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        maxPillsStacks = (int)(maxPillsStacks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        SpawnStackOfPills();
    }

    void SpawnStackOfPills()
    {
        int pillsStackCount = Random.Range(minPillsStacks, maxPillsStacks + 1);

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

        while (spawnedPositions.Count < pillsStackCount && attempts < maxAttempts)
        {
            attempts++;

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

            if (!tooClose)
            {
                GameObject newPillsStack = Instantiate(stackOfPillsPrefab, transform);
                newPillsStack.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                spawnedPositions.Add(spawnPosition);
            }
        }

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Reached maximum attempts while spawning stack of pills. Some may not have been spawned.");
        }
    }
}
