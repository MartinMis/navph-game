using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Bosses
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class BossRoomInitializer : MonoBehaviour
    {
        [Tooltip("Prefab of the socket")]
        [SerializeField] private GameObject socketPrefab;
        
        [Tooltip("Prefab of the boss")]
        [SerializeField] private LampBossController boss;
        
        [Tooltip("Number of sockets to spawn with difficulty 1")]
        [SerializeField] private int initialSocketCount = 4;
        
        [Tooltip("Minimal distance of the sockets")]
        [SerializeField] private float socketDistance = 4;
        
        [Tooltip("How many times to attempt to spawn a socket")]
        [SerializeField] private int spawnAttempts = 10;
    
        void Start()
        {
            SpawnSockets();
        }
        
        /// <summary>
        /// Helper method for spawning the socket in the boss room.
        /// </summary>
        private void SpawnSockets()
        {
            // Adjust socket count according to difficulty
            var actualSocketCount = initialSocketCount + DifficultyManager.Instance.CurrentDifficulty - 1;
            // Calculate damage for individual sockets (+1 to avoid float rounding errors causing not enough damage)
            var socketDamage = boss.MaxHealth / actualSocketCount + 1;
            var boxCollider = GetComponent<BoxCollider2D>();
            if (boxCollider == null)
            {
                Debug.LogError("[BossRoomInitializer] Boss Collider is not set");
                return;
            }
            var width = boxCollider.size.x;
            var height = boxCollider.size.y;
        
            List<Vector3> socketPositions = new List<Vector3>();
            for (var i = 0; i < actualSocketCount; i++)
            {
                var newPosition = new Vector3();
                // Try to spawn the socket several times
                for (var spawnAttempt = 0; spawnAttempt < spawnAttempts; spawnAttempt++)
                {
                    // Generate new socket position and verify its validity
                    newPosition = GetRandomPosition(width, height);
                    var invalidPosition = false;
                    foreach (var position in socketPositions)
                    {
                        if (Vector3.Distance(position, newPosition) < socketDistance)
                        {
                            invalidPosition = true;
                        }
                    }

                    if (!invalidPosition)
                    {
                        break;
                    }
                    
                    // If no valid position was found exit
                    if (spawnAttempt == spawnAttempts - 1)
                    {
                        return;
                    }
                }
                
                // Spawn a new socket at the given position and initialize it
                socketPositions.Add(newPosition);
                var socket = Instantiate(socketPrefab, transform);
                socket.transform.localPosition = newPosition;
                var socketController = socket.GetComponent<SocketController>();
                if (socketController != null)
                {
                    socketController.Initialize(boss, socketDamage);
                }
            }
        }
        
        /// <summary>
        /// Helper function to generate a random position on the wall of the boss room.
        /// </summary>
        /// <param name="width">Width of the room</param>
        /// <param name="height">Height of the room</param>
        /// <returns></returns>
        Vector3 GetRandomPosition(float width, float height)
        {
            // Choose whether to spawn on a horizontal or vertical wall
            bool lockHorizontal = Random.Range(0, 2) == 0;
            // Chose on which of the two walls to spawn
            int side = Random.Range(0, 2);
            if (lockHorizontal)
            {
                float y = Random.Range(-height/2, height/2);
                float x = side == 0 ? -width/2 : width/2;
                return new Vector3(x, y, 0);
            }
            else
            {
                float x = Random.Range(-width/2, width/2);
                float y = side == 0 ? -height/2 : height/2;
                return new Vector3(x, y, 0);
            }
        }
    }
}
