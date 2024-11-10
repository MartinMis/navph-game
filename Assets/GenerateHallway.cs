using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;



public class GenerateHallway : MonoBehaviour
{
    [SerializeField] private GameObject floorObject;
    [SerializeField] private GameObject widnowObject;

    [SerializeField] private int minWindowCount = 10;
    [SerializeField] private int maxWindowCount = 20;
    [SerializeField] private float minLength = 50;
    [SerializeField] private float maxLength = 100;
    [SerializeField] private float minLightRange = 5;
    [SerializeField] private float maxLightRange = 10;

    private float _hallwayWidth;
    private float _hallwayLength;
    void Start()
    {
        GameObject floor = transform.Find("Floor").gameObject;
        float width = floor.GetComponent<SpriteRenderer>().size.x;
        float hallwayLength = Random.Range(minLength, maxLength);
        floor.GetComponent<SpriteRenderer>().size = new Vector2(width, hallwayLength);


        Vector2[] points = new Vector2[]
        {
            new Vector2(-width/2, -hallwayLength/2),
            new Vector2(width/2, -hallwayLength/2),
            new Vector2(width/2, hallwayLength/2),
            new Vector2(-width/2, hallwayLength/2),
            new Vector2(-width/2, -hallwayLength/2)
        };
        floor.GetComponent<EdgeCollider2D>().points = points;

        transform.position += new Vector3(0, hallwayLength/2-width, 0);


        _hallwayWidth = width;
        _hallwayLength = hallwayLength;
        SpawnWindows();
    }

    void SpawnWindows()
    {
        int windowCount = Random.Range(minWindowCount, maxWindowCount);
        float minY = -_hallwayLength/2 + widnowObject.GetComponent<SpriteRenderer>().bounds.size.y/2;
        float maxY = _hallwayLength/2 - widnowObject.GetComponent<SpriteRenderer>().bounds.size.y/2;
        float widthOffset = widnowObject.GetComponent<SpriteRenderer>().bounds.size.x;

        List<float> leftWallWindows = new List<float>();

        for (int i = 0; i < windowCount; i++)
        {
            int side = Random.Range(0, 2);
            GameObject window = Instantiate(widnowObject, transform);
            if (side == 0)
            {
                window.transform.position += new Vector3(-_hallwayWidth/2 + widthOffset, 0, 0);
            }
            else
            {
                window.transform.position += new Vector3(_hallwayWidth/2 - widthOffset, 0, 0);
                window.transform.Rotate(new Vector3(0, 0, 180));
            }

            float windowY = Random.Range(minY, maxY);
            bool repeat = true;
            int maxSpawnAttempts = 25;
            int currentAttempts = 0;
            while (repeat && currentAttempts < maxSpawnAttempts)
            {   
                currentAttempts++;
                repeat = false;
                foreach(float pos in leftWallWindows)
                {
                    if (Mathf.Abs(windowY - pos) < 5)
                    {
                        windowY = Random.Range(minY, maxY);
                        repeat = true;
                        break;
                    }
                }
            }

            if (currentAttempts < maxSpawnAttempts)
            {
                leftWallWindows.Add(windowY);
                window.transform.position += new Vector3(0, windowY,0);
            }
            else
            {
                Destroy(window);
                break;
            }

            

            LightControl lc = window.GetComponent<LightControl>();
            if (lc != null)
            {
                lc.setLightRange(Random.Range(minLightRange, maxLightRange));
            }
            

        }
    }
}
