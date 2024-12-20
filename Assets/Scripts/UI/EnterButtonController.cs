using System;
using Camera;
using UnityEngine;

namespace UI
{
    public class EnterButtonController : MonoBehaviour
    {
        [SerializeField] bool changeCameraHorizontalLock = true;
    
        private Vector3 _targetPosition;
        private Transform _player;
        private FollowPlayer _followingCamera;
    
        public event Action ButtonPressed;

        public void ExecuteInteraction()
        {
            ButtonPressed?.Invoke();
            ResetAllListeners();
        }

        public void ResetAllListeners()
        {
            ButtonPressed = null;
        }

        public bool InteractionIsAssigned()
        {
            if (ButtonPressed == null)
            {
                return false;
            }
            return true;
        }
    
    }
}
