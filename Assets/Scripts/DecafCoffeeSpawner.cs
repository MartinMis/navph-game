using System.Collections.Generic;
using UnityEngine;

public class DecafCoffeeSpawner : MonoBehaviour
{
    [SerializeField] private GameObject decafCoffeePrefab;
    [SerializeField] private int minDecafCoffees = 1;
    [SerializeField] private int maxDecafCoffees = 5;
    [SerializeField] private float minSeparation = 2f;

    void Start()
    {
        SpawnDecafCoffees();
    }

    void SpawnDecafCoffees()
    {
        int coffeeCount = Random.Range(minDecafCoffees, maxDecafCoffees + 1);

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

        while (spawnedPositions.Count < coffeeCount && attempts < maxAttempts)
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
