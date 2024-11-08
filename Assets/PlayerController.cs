using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float health = 0;
    [SerializeField] private TextMeshProUGUI wakeupMeter;
    // Start is called before the first frame update
    void Start()
    {
        wakeupMeter.text = "Wakeup Meter: " + health;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DamagePlayer(float damage)
    {
        health += damage;
        wakeupMeter.text = "Wakeup Meter: " + health;
    }
}
