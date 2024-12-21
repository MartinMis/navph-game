using System;
using Spawners;
using UnityEngine;
using Utility;

namespace Camera
{
    /// <summary>
    /// Functionality of the camera to track moving player through the hallway.
    /// </summary>
    public class FollowPlayer : MonoBehaviour
    {
        [Tooltip("Prefab of the hallway")]
        [SerializeField] private GameObject hallwayPrefab;
        
        [Tooltip("How much can the player move horizontally before being followed")]
        [SerializeField] private float horizontalDeadzone = 0;
        
        [Tooltip("How much the player move vertically before being followed")]
        [SerializeField] private float verticalDeadzone = 0;
        
        [Tooltip("How outside the hallway start can the camera go")]
        [SerializeField] private float startPadding = 50;
        
        [Tooltip("How far outside the hallway end can the camera go")]
        [SerializeField] private float endPadding = 50;
        public bool ignoreVertical = false;
        public bool ignoreHorizontal = true;
    
        private Transform _playerTransform;
        private float _hallwayLength;
        private UnityEngine.Camera _camera;

        void Start()
        {
            _camera = UnityEngine.Camera.main;
            _playerTransform = GameObject.FindGameObjectWithTag(Tags.Player).transform;
            _hallwayLength = hallwayPrefab.GetComponent<GenerateHallway>().HallwayCameraLength;
        }
        void Update()
        {
            // Helper variables
            if (_playerTransform == null) return;

        
            float playerY = _playerTransform.position.y;
            float cameraY = transform.position.y;
            float playerX = _playerTransform.position.x;
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
        * if some coord wasn't changed set it to current position. This is done like
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
            transform.position = ClampPositionToHallway(newPosition);
        }
        
        /// <summary>
        /// Helper method that forces the camera to follow the hallway
        /// </summary>
        /// <param name="pos">Initial position</param>
        /// <returns></returns>
        Vector3 ClampPositionToHallway(Vector3 pos)
        {
            // Calculate the maximal and minimal position
            float maxYHallway = hallwayPrefab.transform.position.y + _hallwayLength/2 + endPadding;
            float minYHallway = hallwayPrefab.transform.position.y - _hallwayLength/2 - startPadding;
            
            // Account for camera dimensions
            if (_camera != null)
            {
                float camHeight = _camera.orthographicSize;
                maxYHallway -= camHeight;
                minYHallway += camHeight;
            }
            
            if (pos.y < minYHallway)
            {
                pos.y = minYHallway;
            }
            else if (pos.y > maxYHallway)
            {
                pos.y = maxYHallway;
            }
            return new Vector3(pos.x, pos.y, pos.z);
        }
    }
}
