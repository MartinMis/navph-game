using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float health = 0;
    [SerializeField] private TextMeshProUGUI wakeupMeter;
    [SerializeField] private float speed ;
    // Start is called before the first frame update
    
    private Rigidbody2D _rigidbody;
    private Vector3 _moveInput;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        wakeupMeter.text = "Wakeup Meter: " + health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _rigidbody.velocity = 10 * speed * Time.fixedDeltaTime * _moveInput;
    }

    public void DamagePlayer(float damage)
    {
        health += damage;
        wakeupMeter.text = "Wakeup Meter: " + health;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
}
