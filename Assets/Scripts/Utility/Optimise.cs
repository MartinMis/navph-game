using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace Utility
{
    /// <summary>
    /// Simple class setting the FPS to 60
    /// </summary>
    public class Optimise : MonoBehaviour
    {
        void Start()
        {
            Application.targetFrameRate = 60;
        }
        
    }
}
