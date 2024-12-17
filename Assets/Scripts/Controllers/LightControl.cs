using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightControl : MonoBehaviour
{
    public void setLightRange(float range)
    {
        Debug.Log("Called");
        GameObject lightObject = transform.Find("Light 2D").gameObject;
        if (lightObject != null)
        {
            Light2D light = lightObject.GetComponent<Light2D>();
            if (light != null)
            {
                light.pointLightOuterRadius = range;
                Debug.Log(light.pointLightOuterRadius);
            }
        }
        
    }
}
