using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Light
{
    // Class for controlling the lights   
    public class LightControl : MonoBehaviour
    {
        [Tooltip("Light to be controlled")]
        [SerializeField] private GameObject lightObject;
        
        /// <summary>
        /// Method for setting the range of the light
        /// </summary>
        /// <param name="range">Range of light to set</param>
        public void SetLightRange(float range)
        {
            if (lightObject != null)
            {
                Light2D light = lightObject.GetComponent<Light2D>();
                if (light != null)
                {
                    light.pointLightOuterRadius = range;
                }
            }
        
        }
    }
}
