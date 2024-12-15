using System.Collections.Generic;
using UnityEngine;

public class CoffeeCupSpawner : MonoBehaviour
{
    [SerializeField] private GameObject coffeeCupPrefab;
    [SerializeField] private int minCoffeeCups = 1;
    [SerializeField] private int maxCoffeeCups = 5;
    [SerializeField] private float minYSeparation = 2f;

    void Start()
    {
        SpawnCoffeeCups();
    }

    void SpawnCoffeeCups()
    {
        int coffeeCupCount = Random.Range(minCoffeeCups, maxCoffeeCups + 1);

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

        while (spawnedPositions.Count < coffeeCupCount && attempts < maxAttempts)
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
