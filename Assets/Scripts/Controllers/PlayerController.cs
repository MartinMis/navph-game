using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float health = 0;
    [SerializeField] private TextMeshProUGUI wakeupMeter;
    [SerializeField] public float speed;
    [SerializeField] private float interactionRadius = 3;
    [SerializeField] private GameObject decafCoffeePrefab; // dictionary
    [SerializeField] private GameObject stylishShadesPrefab;
    // Start is called before the first frame update
    
    private Rigidbody2D _rigidbody;
    private Vector3 _moveInput;

    private float damageReduction = 0f;
    private DamageType reducedDamageType;
    private GameObject equippedItemPrefab;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        wakeupMeter.text = "Wakeup Meter: " + health;
    }
    
    void FixedUpdate()
    {
        _rigidbody.velocity = 10 * speed * Time.fixedDeltaTime * _moveInput;
    }

    public void DamagePlayer(float damage, DamageType damageType = DamageType.None)
    {
        float finalDamage = damage;
        
        
        
        // Account for item effect
        if (equippedItemPrefab != null && damageType == reducedDamageType)
        {
            finalDamage *= (1 - damageReduction);
        }

        health += finalDamage;
        wakeupMeter.text = "Wakeup Meter: " + health;
    }

    public void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
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
                if (distance < minDistance && distance <= interactionRadius)
                {
                    minDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != null)
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
            damageReduction = decafCoffee.GetDamageReduction();
            reducedDamageType = decafCoffee.GetAffectedDamageType();
            equippedItemPrefab = decafCoffeePrefab;
        }
        else if (newItem is StylishShades stylishShades)
        {
            damageReduction = stylishShades.GetDamageReduction();
            reducedDamageType = stylishShades.GetAffectedDamageType();
            equippedItemPrefab = stylishShadesPrefab;
        }

        //Debug.Log("TEST setting");
        //Debug.Log(reducedDamageType);

        UIManager.Instance.UpdateEquippedItemUI(GetItemSprite(newItem));

        Destroy(newItem.gameObject);
    }

    private void DropCurrentItem()
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
