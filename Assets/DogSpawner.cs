using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSpawnerScript : MonoBehaviour
{
    [SerializeField] private GameObject dogPrefab;
    [SerializeField] private int minDogCount = 5;
    [SerializeField] private int maxDogCount = 15;

    void Start()
    {
        CreateDogs();
    }

    void CreateDogs()
    {
        int dogCount = Random.Range(minDogCount, maxDogCount);

        GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
        float hallwayWidth = hallwayGenerator.HallwayWidth;
        float hallwayLength = hallwayGenerator.HallwayLength;

        for (int i = 0; i < dogCount; i++)
        {
            float xPosition = Random.Range(-hallwayWidth / 2, hallwayWidth / 2);
            float yPosition = Random.Range(-hallwayLength / 2, hallwayLength / 2);
            Vector3 dogPosition = new Vector3(xPosition, yPosition, 0);

            GameObject newDog = Instantiate(dogPrefab, transform);
            newDog.transform.localPosition = dogPosition;

            DogController dogController = newDog.GetComponent<DogController>();
            if (dogController != null)
            {
                bool movingRight = Random.Range(0, 2) == 0;
                dogController.SetDirection(movingRight);

                float minXLimit = -hallwayWidth / 2;
                float maxXLimit = hallwayWidth / 2;
                dogController.SetMovementLimits(minXLimit, maxXLimit);
            }
        }
    }
}
