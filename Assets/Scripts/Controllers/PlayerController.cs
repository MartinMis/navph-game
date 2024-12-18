using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class PlayerController : MonoBehaviour
{
    public float health = 0;
    [SerializeField] public float speed;
    [SerializeField] private float interactionRadius = 3;
    public float maxSleepMeter = 100;
    [SerializeField] private GameObject decafCoffeePrefab; // dictionary
    [SerializeField] private GameObject stylishShadesPrefab;
    public bool GodMode = false;
    // Start is called before the first frame update
    
    private Rigidbody2D _rigidbody;
    private Vector3 _moveInput;

    public float damageReduction { get; set; } = 0f;
    public DamageType reducedDamageType { get; set; }
    public GameObject equippedItemPrefab { get; set; }
    
    private bool _canMove = true;
    private bool _canInteract = true;
    
    public event Action OnDeath;
    public event Action OnWakeUpMeterUpdated;
    public event Action<string> OnItemEquipped;

    
    void Start()
    {
        var player = GameObject.FindGameObjectWithTag("Player"); 
        if (player != null && player != gameObject)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(this);
        _rigidbody = GetComponent<Rigidbody2D>();
        CalculatePlayerSpeed();
        maxSleepMeter *= GetSleepMeterModifier();
    }

    void CalculatePlayerSpeed()
    {
        var speedUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Slippers);
        speedUpgrade?.ApplyEffect();
        if (speedUpgrade is PlayerSpeedUpgrade psu)
        {
            speed *= psu.SpeedMultiplier;
        }
    }

    float GetSleepMeterModifier()
    {
        var sleepMeterUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Bear);
        sleepMeterUpgrade?.ApplyEffect();
        if (sleepMeterUpgrade is SleepMeterCapacityUpgrade smcu)
        {
            return smcu.SleepMeterCapacityModifier;
        }
        return 1;
    }

    public void ToggleMovement()
    {
        _canMove = !_canMove;
        _canInteract = !_canInteract;
    }
    

    public void Heal(float healAmount)
    {
        health -= healAmount;
        health = Mathf.Clamp(health, 0, maxSleepMeter);
        OnWakeUpMeterUpdated?.Invoke();
    }

    float GetDamageReduction()
    {
        var lightDamageUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Pyjama);
        lightDamageUpgrade?.ApplyEffect();
        if (lightDamageUpgrade is LightDamageUpgrade ldmg)
        {
            return ldmg.LightDamageModifier;
        }
        return 1;
    }
    
    void FixedUpdate()
    {
        _rigidbody.velocity = 10 * speed * Time.fixedDeltaTime * _moveInput;
    }

    public void DamagePlayer(float damage, DamageType damageType = DamageType.None)
    {
        // If player is currently invincible skip the body of the function
        if (GodMode) return;
        
        float finalDamage = damage;
        
        finalDamage *= GetDamageReduction();
        
        // Account for item effect
        if (equippedItemPrefab != null && damageType == reducedDamageType)
        {
            finalDamage *= (1 - damageReduction);
        }
        
        Debug.Log($"[PlayerController] Taking Damage: {finalDamage}");
        health += finalDamage;

        if (health > maxSleepMeter)
        {
            Debug.Log($"[PlayerController] Player is dead!");
            Die();
        }
        
        OnWakeUpMeterUpdated?.Invoke();
    }

    public void RefreshWakeUpMeter()
    {
        OnWakeUpMeterUpdated?.Invoke();
    }

    public float GetCurrentWakeupMeter()
    {
        return health;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = _canMove ? value.Get<Vector2>() : new Vector2(0, 0);
    }
    void Update()
    {
        HandleInteraction();
    }

    private void HandleInteraction()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
        Interactable closestInteractable = null;
        float minDistance = Mathf.Infinity;

        foreach (Collider2D hit in hits)
        {
            Interactable interactable = hit.GetComponent<Interactable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, interactable.transform.position);
                if (distance < minDistance && distance <= interactionRadius && distance <= interactable.interactionRadius)
                {
                    minDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != null && _canInteract)
        {
            UIManager.Instance.ToggleInteractionPopUp(true, closestInteractable.transform.position,
                callback: (() => closestInteractable.Interact(this)));
        }
        else
        {
            UIManager.Instance.ToggleInteractionPopUp(false, Vector3.zero);
        }
    }

    public void EquipItem(Interactable newItem)
    {
        if (newItem == null) return;

        if (equippedItemPrefab != null)
        {
            DropCurrentItem();
        }
        // newItem onEquipItem - open close principle
        if (newItem is DecafCoffee decafCoffee)     // item class - onEquiItem: implmetuje oddelene
        {
            OnItemEquipped?.Invoke($"Decreased coffee damage by {decafCoffee.GetDamageReduction()}");
            damageReduction = decafCoffee.GetDamageReduction();
            reducedDamageType = decafCoffee.GetAffectedDamageType();
            equippedItemPrefab = decafCoffeePrefab;
        }
        else if (newItem is StylishShades stylishShades)
        {
            OnItemEquipped?.Invoke("Decrease damage taken");
            damageReduction = stylishShades.GetDamageReduction();
            reducedDamageType = stylishShades.GetAffectedDamageType();
            equippedItemPrefab = stylishShadesPrefab;
        }

        //Debug.Log("TEST setting");
        //Debug.Log(reducedDamageType);

        UIManager.Instance.UpdateEquippedItemUI(GetItemSprite(newItem));

        Destroy(newItem.gameObject);
    }

    public void DropCurrentItem()
    {
        if (equippedItemPrefab != null)
        {
            Instantiate(equippedItemPrefab, transform.position, Quaternion.identity);

            equippedItemPrefab = null;
            damageReduction = 0f;
            reducedDamageType = default;

            UIManager.Instance.UpdateEquippedItemUI(null);
        }
    }
    

    private void Die()
    {
        OnDeath?.Invoke();
        DropCurrentItem();
        Destroy(gameObject);
    }


    private Sprite GetItemSprite(Interactable item)
    {
        if (item is DecafCoffee decafCoffee)
        {
            return decafCoffee.GetItemSprite();
        }
        if (item is StylishShades stylishShades)
        {
            return stylishShades.GetItemSprite();
        }
        return null;
    }
}
