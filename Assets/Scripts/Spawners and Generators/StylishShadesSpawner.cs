using System.Collections.Generic;
using UnityEngine;

public class StylishShadesSpawner : MonoBehaviour
{
    [SerializeField] private GameObject stylishShadesPrefab;
    [SerializeField] private int minStylishShades = 1;
    [SerializeField] private int maxStylishShades = 5;
    [SerializeField] private float minSeparation = 2f;

    void Start()
    {
        minStylishShades = (int)(minStylishShades * DifficultyManager.Instance.HallwayLengthCoeficient);
        maxStylishShades = (int)(maxStylishShades * DifficultyManager.Instance.HallwayLengthCoeficient);
        SpawnStylishShades();
    }

    void SpawnStylishShades()
    {
        int shadesCount = Random.Range(minStylishShades, maxStylishShades + 1);

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
