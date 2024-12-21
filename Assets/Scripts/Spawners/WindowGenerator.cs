using System.Collections.Generic;
using Light;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Spawner for the different windows
    /// </summary>
    public class WindowGenerator : MonoBehaviour
    {
        [Tooltip("Prefab of the window")]
        [SerializeField] private GameObject windowPrefab;
        
        [Tooltip("How much should the window be offset from spawnable area")]
        [SerializeField] private float windowOffset = 8;
        
        [Tooltip("Minimal number of windows to spawn")]
        [SerializeField] private int minWindowCount = 1;
        
        [Tooltip("Maximal number of windows to spawn")]
        [SerializeField] private int maxWindowCount = 10;
        
        [Tooltip("How many times should spawn be attempted")]
        [SerializeField] private int maxSpawnAttempts = 20;
        
        [Tooltip("Distance between windows")]
        [SerializeField] private float windowDistance = 5;
        
        [Tooltip("Minimal light range of the window")]
        [SerializeField] private float minLightRange = 5;
        
        [Tooltip("Maximal light range of the window")]
        [SerializeField] private float maxLightRange = 12;
        
        [Tooltip("Should the windows be allowed to spawn oppsite each other")]
        [SerializeField] private bool forbidOppositeWindows = false;
    
        void Start()
        {
            // Adjust for difficulty
            minWindowCount = (int)(minWindowCount * DifficultyManager.Instance.HallwayLengthCoeficient);
            maxWindowCount = (int)(maxWindowCount * DifficultyManager.Instance.HallwayLengthCoeficient);
            CreateWindows();
        }

        void CreateWindows()
        {
            int windowCount = Random.Range(minWindowCount, maxWindowCount);
            
            // Get spawnable area
            GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
            float hallwayWidth = hallwayGenerator.HallwayWidth;
            float hallwayLength = hallwayGenerator.HallwayLength;
        
            float minYPosition = -hallwayLength/2 + windowPrefab.GetComponent<SpriteRenderer>().bounds.size.y/2;
            float maxYPosition = hallwayLength/2 - windowPrefab.GetComponent<SpriteRenderer>().bounds.size.y/2;
            float xOffset = windowPrefab.GetComponent<SpriteRenderer>().bounds.size.x + windowOffset;
        
            List<float> leftWallWindowPositions = new List<float>();
            List<float> rightWallWindowPositions = new List<float>();
            
            // Spawn widnows one by one
            for (int i = 0; i < windowCount; i++)
            {
                Vector3 newWindowLocalPosition = new Vector3(0, 0, 0);
                Vector3 newWindowRotation = new Vector3(0, 0, 0);
                
                // Choose a side to spawn on
                int side = Random.Range(0, 2);
                // Generate spawn position
                if (side == 0)
                {
                    newWindowLocalPosition += new Vector3(-hallwayWidth / 2 + xOffset, 0, 0);
                }
                else
                {
                    newWindowLocalPosition += new Vector3(hallwayWidth / 2 - xOffset, 0, 0);
                    newWindowRotation += new Vector3(0, 0, 180);
                } 
                
                // Verify the spawn postion
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
                    if (forbidOppositeWindows)
                    {
                        rightWallWindowPositions.Add(windowY);
                    }
                }
                else
                {
                    rightWallWindowPositions.Add(windowY);
                    if (forbidOppositeWindows)
                    {
                        leftWallWindowPositions.Add(windowY);
                    }
                }
                
                // If position is good spawn and initialize the window
                newWindowLocalPosition += new Vector3(0, windowY, 0);
                GameObject newWindow = Instantiate(windowPrefab, new Vector3(0, 0, 0), Quaternion.identity);
                newWindow.transform.SetParent(transform);
                newWindow.transform.localPosition = newWindowLocalPosition;
                newWindow.transform.Rotate(newWindowRotation);

                LightControl lc = newWindow.GetComponent<LightControl>();
                if (lc != null)
                {
                    lc.SetLightRange(Random.Range(minLightRange, maxLightRange));
                }
            }
        }
    }
}
