using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class RoofLightController : MonoBehaviour
{
    [SerializeField] private float radius;
    [SerializeField] private float damage;
    void Start()
    {
        ModifyRadius(radius);
    }

    void Update()
    {
        
    }

    void OnValidate()
    {
        ModifyRadius(radius);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().DamagePlayer(damage);
        }
    }

    public void ModifyRadius(float newRadius)
    {
        GetComponent<CircleCollider2D>().radius = radius;
        GetComponent<Light2D>().pointLightOuterRadius = newRadius;
    }
}
