using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateFloor : MonoBehaviour
{
    [SerializeField]
    private GameObject floorTile;

    [SerializeField]
    private int numberOfRepeats;

    [SerializeField]
    private float spacing;
    void Start()
    {
        for (int i = 0; i < numberOfRepeats; i++){
            Vector3 spawnPosition = new Vector3(0, i*spacing, 0);
            Instantiate(floorTile, spawnPosition, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
