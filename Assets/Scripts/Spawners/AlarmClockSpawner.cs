using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Class for spawning alarm clocks
    /// </summary>
    public class AlarmClockSpawner : MonoBehaviour
    {
        [Tooltip("Alarm clock prefab")]
        [SerializeField] private GameObject alarmClockPrefab;
        
        [Tooltip("Minimal number of alarm clocks to spawn")]
        [SerializeField] private int minAlarmClocks = 1;
        
        [Tooltip("Maximal number of alarm clocks to spawn")]
        [SerializeField] private int maxAlarmClocks = 5;
        
        [Tooltip("How far should the alarm clock be from each other")]
        [SerializeField] private float minYSeparation = 2f;

        void Start()
        {
            // Apply difficulty scaling
            minAlarmClocks = (int)(minAlarmClocks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxAlarmClocks = (int)(maxAlarmClocks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            SpawnAlarmClocks();
        }
        
        /// <summary>
        /// Method for spawning the alarm clocks
        /// </summary>
        void SpawnAlarmClocks()
        {
            int alarmClockCount = Random.Range(minAlarmClocks, maxAlarmClocks + 1);

            GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
            if (hallwayGenerator == null)
            {
                Debug.LogError("[AlarmClockSpawner] GenerateHallway component not found on the hallway GameObject.");
                return;
            }
            // Get the hallway dimensions
            float hallwayWidth = hallwayGenerator.HallwayWidth;
            float hallwayLength = hallwayGenerator.HallwayLength;

            List<Vector2> spawnedPositions = new List<Vector2>();

            int attempts = 0;
            int maxAttempts = 100;
            
            // Attempt to spawn alarm clocks in a loop
            while (spawnedPositions.Count < alarmClockCount && attempts < maxAttempts)
            {
                attempts++;
                
                // Generate new alarm clock position
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
                
                // if alarm clocks are not too close spawn them
                if (!tooClose)
                {
                    GameObject newAlarmClock = Instantiate(alarmClockPrefab, transform);
                    newAlarmClock.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("[AlarmClockSpawner] Reached maximum attempts while spawning alarm clocks. Some alarm clocks may not have been spawned.");
            }
        }
    }
}
