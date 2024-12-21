using System.Collections.Generic;
using Light;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Class for generating the roof lights
    /// </summary>
    public class RoofLightGenerator : MonoBehaviour
    {
        [Tooltip("Prefab of the roof light")]
        [SerializeField] private GameObject roofLightPrefab;
        
        [Tooltip("Minimal allowable radius for the light")]
        [SerializeField] private float minRadius;
        
        [Tooltip("Maximal allowable radius for the light")]
        [SerializeField] private float maxRadius;
        
        [Tooltip("Number of lights to spawn")]
        [SerializeField] private int lightCount;
        
        [Tooltip("Distance between spawned lights")]
        [SerializeField] private float lightDistance;
        
        [Tooltip("How many times to try and spawn the lights")]
        [SerializeField] private int maxSpawnAttempts = 10;
    
        void Start()
        {
            // Adjust initial values based on current difficulty level
            lightCount = (int)(lightCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            
            // Get hallway size
            float hallwayWidth = GetComponent<GenerateHallway>().HallwayWidth;
            float hallwayLength = GetComponent<GenerateHallway>().HallwayLength;
            List<Vector3> roofLightPositions = new List<Vector3>();
            
            // Attempt to spawn lights one by one
            for (int j = 0; j < lightCount; j++)
            {
                Vector3 roofLightPosition = new Vector3(0, 0, 0);
                float radius = Random.Range(minRadius, maxRadius);
                for (int i = 0; i < maxSpawnAttempts; i++)
                {
                    // Try to generate new position for the lights
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
                    
                    // If no valid position was found exit
                    if (i == maxSpawnAttempts - 1)
                    {
                        return;
                    }
                }
                
                // Spawn the lights and adjust their size
                roofLightPositions.Add(roofLightPosition);
                GameObject rl = Instantiate(roofLightPrefab, transform);
                rl.GetComponent<RoofLightController>().ModifyRadius(radius);
                Debug.Log("Radius: " + radius);
                rl.transform.localPosition = roofLightPosition;
            }
        }
    
    }
}
