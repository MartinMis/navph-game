using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        else
        {
            return true;
        }
    }
    
}
