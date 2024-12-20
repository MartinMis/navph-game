using Camera;
using Controllers;
using Items_and_Consumables;
using UnityEngine;

namespace Gameplay
{
    public class TeleportPlayer : Interactable
    {
        [SerializeField] bool changeCameraHorizontalLock = true;
        [SerializeField] Vector3 targetPosition;
        [SerializeField] FollowPlayer followingCamera;
    
        private Transform _player;

        private void Awake()
        {
            if (UnityEngine.Camera.main == null)
            {
                Debug.LogWarning("No main camera found");
                return;
            }
        
            _player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        public override void Interact(PlayerController player)
        {
            _player.position = targetPosition;
            if (changeCameraHorizontalLock)
            {
                followingCamera.transform.position = targetPosition;
                followingCamera.ignoreHorizontal = !followingCamera.ignoreHorizontal;
            }
        }
    }
}
