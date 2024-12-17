using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SocketController : Interactable
{
    public LampBossController lampBoss;
    public float damage = 50;
    private PlayerController _player;
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public override void Interact(PlayerController player)
    {
        lampBoss.TakeDamage(damage);
        Destroy(gameObject);
    }
    
}
