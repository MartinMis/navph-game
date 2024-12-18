using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(BoxCollider2D))]
public class BoosRoomInitializer : MonoBehaviour
{
    [SerializeField] private GameObject socketPrefab;
    [SerializeField] private LampBossController boss;
    [SerializeField] private int initialSocketCount = 4;
    [SerializeField] private float socketDistance = 4;
    [SerializeField] private int spawnAttempts = 10;
    
    private void Start()
    {
        SpawnSockets();
    }

    private void SpawnSockets()
    {
        // Adjust socket count according to difficulty
        var actualSocketCount = initialSocketCount + DifficultyManager.Instance.currentDifficulty - 1;
        // Calculate damage for individual sockets (+1 to avoid float rounding errors causing not enough damage)
        var socketDamage = boss.maxHealth / actualSocketCount + 1;
        var boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("[BossRoomInitializer] Boss Collider is not set");
            return;
        }
        var width = boxCollider.size.x;
        var height = boxCollider.size.y;
        
        List<Vector3> socketPositions = new List<Vector3>();
        for (int i = 0; i < actualSocketCount; i++)
        {
            var newPosition = new Vector3();
            for (var spawnAttempt = 0; spawnAttempt < spawnAttempts; spawnAttempt++)
            {
                newPosition = GetRandomPosition(width, height);
                bool invalidPosition = false;
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

                if (spawnAttempt == spawnAttempts - 1)
                {
                    return;
                }
            }
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

    private Vector3 GetRandomPosition(float width, float height)
    {
        bool lockHorizontal = Random.Range(0, 2) == 0;
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
