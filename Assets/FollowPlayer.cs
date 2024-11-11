using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private bool ignoreVertical = false;
    [SerializeField] private bool ignoreHorizontal = true;
    [SerializeField] private float horizontalDeadzone = 0;
    [SerializeField] private float verticalDeadzone = 0;

    // Update is called once per frame
    void Update()
    {
        // Helper variables
        float playerY = playerTransform.position.y;
        float cameraY = transform.position.y;
        float playerX = playerTransform.position.x;
        float cameraX = transform.position.x;
        // Starting camera position
        Vector3 newPosition = new Vector3(0, 0, -10);

        if (!ignoreVertical)
        {
            if (Math.Abs(playerY - cameraY) > verticalDeadzone)
            {
                if (playerY > cameraY)
                {
                    newPosition += new Vector3(0, playerY - verticalDeadzone, 0);
                }
                else
                {
                    newPosition += new Vector3(0, playerY + verticalDeadzone, 0);
                }
            }
        }
        if (!ignoreHorizontal)
        {
            
            if (Math.Abs(playerX - cameraX) > horizontalDeadzone)
            {
                if (playerX > cameraX)
                {
                    newPosition += new Vector3(playerX - horizontalDeadzone, 0, 0);
                }
                else
                {
                    newPosition += new Vector3(playerX + horizontalDeadzone, 0, 0);
                }
            }
        }
        /* 
        * if some coord wasnt changed set it to current position. This is done like
        * this because if we set it at the start we could possibly double add tp 
        * position causing all kinds of weird stuff.
        */
        if (newPosition.x == 0)
        {
            newPosition.x = cameraX;
        }
        if (newPosition.y == 0)
        {
            newPosition.y = cameraY;
        }
        transform.position = newPosition;
    }
}
