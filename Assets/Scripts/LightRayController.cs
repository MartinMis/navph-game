using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightRayController : MonoBehaviour
{
    public int lightRayCount = 8;
    [SerializeField] GameObject lightRayPrefab;

    private float _rayWidth = 0;

    void Start()
    {
        CreateLightRays();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateLightRays()
    {
        _rayWidth = 180/lightRayCount;
        float rayAngle = _rayWidth * 2;
        for (int i = 0; i < lightRayCount; i++)
        {
            GameObject newLightRay = Instantiate(lightRayPrefab, transform);
            newLightRay.transform.localEulerAngles = new Vector3(0, 0, rayAngle*i);
            Light2D newLightRayLight2D = newLightRay.GetComponent<Light2D>();
            newLightRayLight2D.pointLightOuterAngle = _rayWidth;
            newLightRayLight2D.pointLightInnerAngle = _rayWidth * 0.9f;
        }
    }
}
