using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SunriseBarController : MonoBehaviour
{
    [SerializeField] Image sunriseBar;
    [SerializeField] RunTimer runTimer;

    void Awake()
    {
        runTimer.OnUpdate += ChangeFill;
    }

    void ChangeFill(float fill)
    {
        sunriseBar.fillAmount = fill;
    }
}
