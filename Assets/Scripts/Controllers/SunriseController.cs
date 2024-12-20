using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Controllers
{
    public class SunriseController : MonoBehaviour
    {
        private Light2D _light;
        private float _lightLevel;
        public float LightLevel
        {
            get => _lightLevel;
            set
            {
                Debug.Log("Setting light level to " + value);
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
