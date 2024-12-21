using System.Collections.Generic;
using Gameplay;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Utility;

namespace Light
{
    /// <summary>
    /// Class giving light the ability to damage player
    /// </summary>
    [RequireComponent(typeof(Light2D))]
    public class DealLightDamage : MonoBehaviour
    {
        [Tooltip("How much damage should the light do")]
        [SerializeField] private float damage;
        
        [Tooltip("How many rays should the light cast")]
        [SerializeField] private int rayCount;
    
        private Light2D _light;
        private float _startAngle;
        private float _angleStep;

        void Start()
        {   
            _light = GetComponent<Light2D>();
            _angleStep = _light.pointLightOuterAngle/rayCount;
            _startAngle = _light.pointLightOuterAngle/2;
        }
    
        void Update()
        {
            // Cast rays in the angles of the light
            List<Vector3> rayDirections = new List<Vector3>();
            for (int i = 0; i <= rayCount; i++)
            {
                Vector3 rayDirection = Quaternion.AngleAxis(_startAngle - i * _angleStep, new Vector3(0,0,1)) * transform.up;
                rayDirections.Add(rayDirection);
                Debug.DrawRay(transform.position,  rayDirection *  _light.pointLightOuterRadius,Color.red);
            }
        
            // Check each ray for collision with the player
            foreach (Vector3 rayDirection in rayDirections)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _light.pointLightOuterRadius);
                if (hit.collider)
                {
                    if (hit.collider.CompareTag(Tags.Player))
                    {
                        hit.collider.GetComponent<PlayerController>().DamagePlayer(damage*Time.deltaTime, DamageType.Light);
                    }
                }
            
            }
        }
    }
}