using System.Collections;
using System.Collections.Generic;
using Utility;
using UnityEngine;

public class ActivateNearPlayer : MonoBehaviour
{
    [SerializeField] private GameObject light;

    void Start()
    {
        light.SetActive(false);
    }
    

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            light.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            light.SetActive(false);
        }
    }
}
