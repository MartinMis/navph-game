using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureSpawner : MonoBehaviour
{
    [SerializeField] List<GameObject> furniturePrefab;
    [SerializeField] GameObject couchPrefab;
    [SerializeField] float couchSpawnRate;
    [SerializeField] float furnitureDistance = 5;
    [SerializeField] int spawnAttempts = 20;
    [SerializeField] int furnitureCount;
    [SerializeField] float wallSnapDistance;
    
    private List<Vector3> furniturePositions = new ();
    void Start()
    {
        furnitureCount = (int)(furnitureCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        
        GenerateHallway hallway = GetComponent<GenerateHallway>();
        float hallwayLength = hallway.HallwayLength;
        float hallwayWidth = hallway.HallwayWidth;

        for (int i = 0; i < furnitureCount; i++)
        {
            GameObject randomFurniture;
            if (Random.Range(0.0f, 1.0f) <= couchSpawnRate)
            {
                randomFurniture = couchPrefab;
            }
            else
            {
                randomFurniture = furniturePrefab[Random.Range(0, furniturePrefab.Count)];
            }
            Vector3 spawnPosition = new Vector3(0, 0, 0);
            for (int j = 0; j < spawnAttempts; j++)
            {
                spawnPosition = HallwaySpawner.SpawnPosition(hallwayWidth, hallwayLength, 0, 5);

                if (spawnPosition.x > hallwayWidth/2 - wallSnapDistance)
                {
                    spawnPosition.x = hallwayWidth/2;
                }
                else if (spawnPosition.x < -hallwayWidth/2 + wallSnapDistance)
                {
                    spawnPosition.x = -hallwayWidth/2;
                }
                
                bool tooClose = false;
                if (furniturePositions.Count > 0)
                {
                    foreach (Vector3 pos in furniturePositions)
                    {
                        if (Vector3.Distance(spawnPosition, pos) < furnitureDistance)
                        {
                            tooClose = true;
                        }
                    }

                }
                if (!tooClose)
                {
                    furniturePositions.Add(spawnPosition);
                    break;
                }

                if (j == spawnAttempts - 1)
                {
                    return;
                }
            }
            
            GameObject newFurniture  = Instantiate(randomFurniture, transform);
            newFurniture.transform.localPosition = spawnPosition;

        }
        
        
        
    }
}
