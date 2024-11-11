using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomToFit : MonoBehaviour
{
    [SerializeField] private Transform targetObject;
    [SerializeField] private float padding;
    void Start()
    {
        float screenAspect = (float)Screen.width / (float)Screen.height;
        float targetWidth = targetObject.GetComponent<Renderer>().bounds.size.x + padding;
        GetComponent<Camera>().orthographicSize = targetWidth / (2*screenAspect);
    }
}
