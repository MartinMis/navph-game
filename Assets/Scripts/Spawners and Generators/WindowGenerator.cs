using System.Collections.Generic;
using UnityEngine;

public class WindowGenerator : MonoBehaviour
{
    [SerializeField] private GameObject windowPrefab;
    [SerializeField] private float windowOffset = 8;
    [SerializeField] private int minWindowCount = 1;
    [SerializeField] private int maxWindowCount = 10;
    [SerializeField] private int maxSpawnAttempts = 20;
    [SerializeField] private float windowDistance = 5;
    [SerializeField] private float minLightRange = 5;
    [SerializeField] private float maxLightRange = 12;
    
    void Start()
    {
        minWindowCount = (int)(minWindowCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        maxWindowCount = (int)(maxWindowCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        minLightRange *= DifficultyManager.Instance.HallwaySpawnRateCoeficient;
        maxLightRange *= DifficultyManager.Instance.HallwaySpawnRateCoeficient;
        CreateWindows();
    }

    void CreateWindows()
    {
        int windowCount = Random.Range(minWindowCount, maxWindowCount);
        
        GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
        float hallwayWidth = hallwayGenerator.HallwayWidth;
        float hallwayLength = hallwayGenerator.HallwayLength;
        
        float minYPosition = -hallwayLength/2 + windowPrefab.GetComponent<SpriteRenderer>().bounds.size.y/2;
        float maxYPosition = hallwayLength/2 - windowPrefab.GetComponent<SpriteRenderer>().bounds.size.y/2;
        float xOffset = windowPrefab.GetComponent<SpriteRenderer>().bounds.size.x + windowOffset;
        
        List<float> leftWallWindowPositions = new List<float>();
        List<float> rightWallWindowPositions = new List<float>();

        for (int i = 0; i < windowCount; i++)
        {
            Vector3 newWindowLocalPosition = new Vector3(0, 0, 0);
            Vector3 newWindowRotation = new Vector3(0, 0, 0);

            int side = Random.Range(0, 2);
            if (side == 0)
            {
                newWindowLocalPosition += new Vector3(-hallwayWidth / 2 + xOffset, 0, 0);
            }
            else
            {
                newWindowLocalPosition += new Vector3(hallwayWidth / 2 - xOffset, 0, 0);
                newWindowRotation += new Vector3(0, 0, 180);
            }

            float windowY = Random.Range(minYPosition, maxYPosition);
            for (int spawnAttempt = 0; spawnAttempt < maxSpawnAttempts; spawnAttempt++)
            {
                bool obstructed = false;
                if (side == 0)
                {
                    foreach (float pos in leftWallWindowPositions)
                    {
                        if (Mathf.Abs(windowY - pos) < windowDistance)
                        {
                            windowY = Random.Range(minYPosition, maxYPosition);
                            obstructed = true;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (float pos in rightWallWindowPositions)
                    {
                        if (Mathf.Abs(windowY - pos) < windowDistance)
                        {
                            windowY = Random.Range(minYPosition, maxYPosition);
                            obstructed = true;
                            break;
                        }
                    }
                }

                if (!obstructed)
                {
                    break;
                }

                if (spawnAttempt == maxSpawnAttempts - 1)
                {
                    Debug.Log("Cannot spawn anymore windows! Max spawn attempts reached!");
                    return;
                }
            }

            if (side == 0)
            {
                leftWallWindowPositions.Add(windowY);
            }
            else
            {
                rightWallWindowPositions.Add(windowY);
            }

            newWindowLocalPosition += new Vector3(0, windowY, 0);
            GameObject newWindow = Instantiate(windowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            newWindow.transform.SetParent(transform);
            newWindow.transform.localPosition = newWindowLocalPosition;
            newWindow.transform.Rotate(newWindowRotation);

            LightControl lc = newWindow.GetComponent<LightControl>();
            if (lc != null)
            {
                lc.setLightRange(Random.Range(minLightRange, maxLightRange));
            }
        }
    }
}
