using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utility;

public class ScreamerTrigger : MonoBehaviour
{
    [SerializeField] private GameObject screamerPrefab;
    [SerializeField] private Transform spawnObject;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player))
        {
            var existingScreamer = GameObject.FindGameObjectWithTag(Tags.Screamer);
            if (existingScreamer == null)
            {
                var screamer = Instantiate(screamerPrefab, spawnObject);
                screamer.transform.rotation = Quaternion.Euler(0, 0, 0);
            }
        }
    }
}
