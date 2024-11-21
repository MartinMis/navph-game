using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class RoofLightController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float radius;
    [SerializeField] private float damage;
    
    private bool _dealDamage = false;
    private PlayerController _playerController;
    void Start()
    {
        ModifyRadius(radius);
        _playerController = player.GetComponent<PlayerController>();
    }

    void Update()
    {
        if (_dealDamage)
        {
            _playerController.DamagePlayer(damage);
        }
    }

    void OnValidate()
    {
        ModifyRadius(radius);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dealDamage = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            _dealDamage = false;
        }
    }

    public void ModifyRadius(float newRadius)
    {
        Debug.Log("Setting Radius to: " + newRadius);
        radius = newRadius;
        GetComponent<CircleCollider2D>().radius = newRadius;
        GetComponent<Light2D>().pointLightOuterRadius = newRadius;
    }
}
