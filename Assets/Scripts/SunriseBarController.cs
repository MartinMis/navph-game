using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using Bosses;
using UnityEngine;
using UnityEngine.UI;

public class SunriseBarController : MonoBehaviour
{
    [SerializeField] Text percentageText;

    void Awake()
    {
        if (RunTimer.Instance.Disabled)
        {
            Disable();
        }
        else
        {
            GameObject.FindWithTag("Boss").GetComponent<LampBossController>().OnDeath += Disable;
            RunTimer.Instance.OnUpdate += ChangePercentage;
        }
    }

    void ChangePercentage(float percentage)
    {
        Debug.Log($"Percentage: {percentage}");
        percentageText.text = Mathf.RoundToInt(percentage*100) + "%";
    }

    void Disable()
    {
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        RunTimer.Instance.OnUpdate -= ChangePercentage;
    }
}
