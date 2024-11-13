using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class DamagePlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private float damage;
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
        List<Vector3> rayDirections = new List<Vector3>();
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 rayDirection = Quaternion.AngleAxis(_startAngle - i * _angleStep, new Vector3(0,0,1)) * transform.up;
            rayDirections.Add(rayDirection);
            Debug.DrawRay(transform.position,  rayDirection *  _light.pointLightOuterRadius,Color.red);
        }
        

        foreach (Vector3 rayDirection in rayDirections)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, rayDirection, _light.pointLightOuterRadius);
            if (hit.collider)
            {
                if (hit.collider.CompareTag("Player"))
                {
                    hit.collider.GetComponent<PlayerController>().DamagePlayer(damage + Time.deltaTime);
                }
            }
            
        }
    }
}