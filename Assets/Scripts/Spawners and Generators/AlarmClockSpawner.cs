using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Spawners_and_Generators
{
    public class AlarmClockSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject alarmClockPrefab;
        [SerializeField] private int minAlarmClocks = 1;
        [SerializeField] private int maxAlarmClocks = 5;
        [SerializeField] private float minYSeparation = 2f;

        void Start()
        {
            minAlarmClocks = (int)(minAlarmClocks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            maxAlarmClocks = (int)(maxAlarmClocks * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            SpawnAlarmClocks();
        }

        void SpawnAlarmClocks()
        {
            int alarmClockCount = Random.Range(minAlarmClocks, maxAlarmClocks + 1);

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

            while (spawnedPositions.Count < alarmClockCount && attempts < maxAttempts)
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
                    GameObject newAlarmClock = Instantiate(alarmClockPrefab, transform);
                    newAlarmClock.transform.localPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

                    spawnedPositions.Add(spawnPosition);
                }
            }

            if (attempts >= maxAttempts)
            {
                Debug.LogWarning("Reached maximum attempts while spawning alarm clocks. Some alarm clocks may not have been spawned.");
            }
        }
    }
}
