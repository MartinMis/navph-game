using Camera;
using Interactables;
using UnityEngine;
using Utility;

namespace Gameplay
{
    /// <summary>
    /// Component giving object the ability to teleport player to the desired location
    /// </summary>
    public class TeleportPlayer : Interactable
    {
        [Tooltip("Should the teleport also unlock the camera")]
        [SerializeField] bool changeCameraHorizontalLock = true;
        
        [Tooltip("Where to teleport to")]
        [SerializeField] Vector3 targetPosition;
        
        [Tooltip("Camera following the player")]
        [SerializeField] FollowPlayer followingCamera;
        
        [Tooltip("Should the teleport disable screamer")]
        [SerializeField] bool disableScreamer;
    
        private Transform _player;

        private void Awake()
        {
            if (UnityEngine.Camera.main == null)
            {
                return;
            }
        
            _player = GameObject.FindGameObjectWithTag(Tags.Player).transform;
        }
        
        /// <summary>
        /// Override of the <c>Interact</c> method for teleporting the player
        /// </summary>
        /// <param name="player">PlayerController reference</param>
        public override void Interact(PlayerController player)
        {
            // Teleport player
            _player.position = targetPosition;
            // Adjust camera if necessary
            if (changeCameraHorizontalLock)
            {
                followingCamera.transform.position = targetPosition;
                followingCamera.ignoreHorizontal = !followingCamera.ignoreHorizontal;
            }

            if (disableScreamer)
            {
                Destroy(GameObject.FindGameObjectWithTag(Tags.Screamer));
            }
        }
    }
}
