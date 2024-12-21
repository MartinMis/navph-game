using System.Collections;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using Utility;

namespace Spawners
{
    /// <summary>
    /// Spawner for the furniture objects
    /// </summary>
    public class FurnitureSpawner : MonoBehaviour
    {
        [Tooltip("List of furniture prefabs")]
        [SerializeField] List<GameObject> furniturePrefab;
        
        [Tooltip("Couch prefab")]
        [SerializeField] GameObject couchPrefab;
        
        [Tooltip("How often should the couch spawn")]
        [SerializeField] float couchSpawnRate;
        
        [Tooltip("Minimal distance between the pieces of furniture")]
        [SerializeField] float furnitureDistance = 5;
       
        [Tooltip("Number of times to attempt to spawn the furniture")]
        [SerializeField] int spawnAttempts = 20;
        
        [Tooltip("How much furniture should spawn")]
        [SerializeField] int furnitureCount;
        
        [Tooltip("Distance from which the furniture should align with the wall")]
        [SerializeField] float wallSnapDistance;
    
        private List<Vector3> _furniturePositions = new ();
        private List<GameObject> _spawnedFurniture = new ();
        void Start()
        {
            furnitureCount = (int)(furnitureCount * DifficultyManager.Instance.HallwaySpawnRateCoeficient);
            // Get hallway dimmensions
            GenerateHallway hallway = GetComponent<GenerateHallway>();
            float hallwayLength = hallway.HallwayLength;
            float hallwayWidth = hallway.HallwayWidth;
            
            // Try to spawn the furniture one by one
            for (int i = 0; i < furnitureCount; i++)
            {   
                // Decide if couch should be spawned or some other furniture
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
                // Attempt to find a suitable spawn position
                for (int j = 0; j < spawnAttempts; j++)
                {
                    spawnPosition = SpawnPosition(hallwayWidth, hallwayLength, 0, 5);
                    
                    // Snap furniture to the wall if close 
                    if (spawnPosition.x > hallwayWidth/2 - wallSnapDistance)
                    {
                        spawnPosition.x = hallwayWidth/2;
                    }
                    else if (spawnPosition.x < -hallwayWidth/2 + wallSnapDistance)
                    {
                        spawnPosition.x = -hallwayWidth/2;
                    }
                
                    bool tooClose = false;
                    if (_furniturePositions.Count > 0)
                    {
                        foreach (Vector3 pos in _furniturePositions)
                        {
                            if (Vector3.Distance(spawnPosition, pos) < furnitureDistance)
                            {
                                tooClose = true;
                            }
                        }

                    }
                    
                    
                    
                    if (!tooClose)
                    {
                        _furniturePositions.Add(spawnPosition);
                        break;
                    }
                    // If no viable position was found exit
                    if (j == spawnAttempts - 1)
                    {
                        return;
                    }
                }
                // Spawn new furniture
                GameObject newFurniture  = Instantiate(randomFurniture, transform);
                newFurniture.transform.localPosition = spawnPosition;
                _spawnedFurniture.Add(newFurniture);
            }
            // Delete any furniture overlapping with other things in the hallway
            StartCoroutine(RemoveOverlapping());
        }
        
        /// <summary>
        /// Coroutine for deleting the overlapping furniture. Inspired by
        /// https://discussions.unity.com/t/how-to-wait-a-certain-amount-of-seconds-in-c/192244
        /// </summary>
        /// <remarks>
        /// Future implementation should use a more robust solution for overlapping
        /// </remarks>
        private IEnumerator RemoveOverlapping()
        {
            yield return new WaitForFixedUpdate();
            foreach (var furniture in _spawnedFurniture)
            {
                var spawnPosition = furniture.transform.localPosition;
                var circleCenter = new Vector2(transform.position.x + spawnPosition.x, transform.position.y + spawnPosition.y);
                Collider2D[] colliders = Physics2D.OverlapCircleAll(circleCenter, 1f);
                foreach (Collider2D collider in colliders)
                {
                    if (!collider.gameObject.CompareTag(Tags.Background) && collider.gameObject != furniture.gameObject)
                    {
                        Destroy(furniture);
                        break;
                    }
                }
            }
        }
        
        /// <summary>
        /// Helper method for generating the spawn positions
        /// </summary>
        /// <param name="width">Width of the hallway</param>
        /// <param name="height">Height of the hallway</param>
        /// <param name="widthPadding">Padding for width</param>
        /// <param name="heightPadding">Padding for heights</param>
        /// <returns></returns>
        private Vector3 SpawnPosition (float width, float height, float widthPadding, float heightPadding)
        {
            float xPosition = Random.Range((-width+widthPadding)/2, (width-widthPadding)/2);
            float yPosition = Random.Range((-height+heightPadding)/2, (height-heightPadding)/2);
        
            return new Vector3(xPosition, yPosition, 0);
        }
        
    }
    
}
