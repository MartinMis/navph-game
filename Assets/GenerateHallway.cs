using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;



public class GenerateHallway : MonoBehaviour
{
    [SerializeField] private float minLength = 50;
    [SerializeField] private float maxLength = 100;

    public float HallwayWidth {get; private set;}
    public float HallwayLength {get; private set;}
    
    void Start()
    {
        GameObject floor = transform.Find("Floor").gameObject;
        HallwayWidth = floor.GetComponent<SpriteRenderer>().size.x;
        HallwayLength = Random.Range(minLength, maxLength);
        floor.GetComponent<SpriteRenderer>().size = new Vector2(HallwayWidth, HallwayLength);


        Vector2[] points = 
        {
            new (-HallwayWidth/2, -HallwayLength/2),
            new (HallwayWidth/2, -HallwayLength/2),
            new (HallwayWidth/2, HallwayLength/2),
            new (-HallwayWidth/2, HallwayLength/2),
            new (-HallwayWidth/2, -HallwayLength/2)
        };
        floor.GetComponent<EdgeCollider2D>().points = points;
        transform.position += new Vector3(0, HallwayLength/2-HallwayWidth, 0);
    }
}
