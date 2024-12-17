using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class RoofLightGenerator : MonoBehaviour
{
    [SerializeField] private GameObject roofLightPrefab;
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private int lightCount;
    [SerializeField] private float lightDistance;
    [SerializeField] private int maxSpawnAttempts = 10;
    
    void Start()
    {
        // Adjust initial values based on current difficulty level
        minRadius *= DifficultyManager.Instance.LightSizeCoeficient;
        maxRadius *= DifficultyManager.Instance.LightSizeCoeficient;
        lightCount = (int)(lightCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
        
        float hallwayWidth = GetComponent<GenerateHallway>().HallwayWidth;
        float hallwayLength = GetComponent<GenerateHallway>().HallwayLength;
        Debug.Log("Hallway width: " + hallwayWidth);
        Debug.Log("Hallway length: " + hallwayLength);
        List<Vector3> roofLightPositions = new List<Vector3>();

        for (int j = 0; j < lightCount; j++)
        {
            Vector3 roofLightPosition = new Vector3(0, 0, 0);
            float radius = Random.Range(minRadius, maxRadius);
            for (int i = 0; i < maxSpawnAttempts; i++)
            {
                float xPos = Random.Range(-hallwayWidth/2 + radius, hallwayWidth/2 - radius);
                float yPos = Random.Range(-hallwayLength/2 + radius, hallwayLength/2 - radius);
                roofLightPosition = new Vector3(xPos, yPos, 0);
            
                bool validPosition = true;
                foreach (Vector3 roofLight in roofLightPositions)
                {
                    if (Vector3.Distance(roofLight, roofLightPosition) < lightDistance)
                    {
                        validPosition = false;
                        xPos = Random.Range(-hallwayWidth/2 + radius, hallwayWidth/2 - radius);
                        yPos = Random.Range(-hallwayLength/2 + radius, hallwayLength/2 - radius);
                        roofLightPosition = new Vector3(xPos, yPos, 0);
                    }
                }

                if (validPosition)
                {
                    break;
                }

                if (i == maxSpawnAttempts - 1)
                {
                    return;
                }
            }
            roofLightPositions.Add(roofLightPosition);
            GameObject rl = Instantiate(roofLightPrefab, transform);
            rl.GetComponent<RoofLightController>().ModifyRadius(radius);
            Debug.Log("Radius: " + radius);
            rl.transform.localPosition = roofLightPosition;
        }
    }
    
}
