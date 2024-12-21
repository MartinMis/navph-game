using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Light
{
    /// <summary>
    /// Class adjusting the global light level according to sunrise timer
    /// </summary>
    public class SunriseController : MonoBehaviour
    {
        private Light2D _light;
        private float _lightLevel;
        public float LightLevel
        {
            get => _lightLevel;
            set
            {
                _lightLevel = value;
                _light.intensity = _lightLevel;
            }
        }
        void Awake()
        {
            _light = GetComponent<Light2D>();
            LightLevel = _light.intensity;
        }
        
    }
}
