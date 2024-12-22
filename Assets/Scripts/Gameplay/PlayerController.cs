using System;
using Interactables;
using Managers;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Upgrades;
using Utility;

namespace Gameplay
{
    /// <summary>
    /// Main controller for the player
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [Tooltip("How fast should the player move")]
        [SerializeField] public float speed;
        
        [Tooltip("In what range should the player be able to interact with things")]
        [SerializeField] private float interactionRadius = 3;
        
        [Header("Usable items")]
        [Tooltip("Decaf Coffee prefab")]
        [SerializeField] private GameObject decafCoffeePrefab; // dictionary
        
        [Tooltip("Stylish Shades prefab")]
        [SerializeField] private GameObject stylishShadesPrefab;
        
        public bool godMode;
        public float maxSleepMeter = 100;
        
        private Rigidbody2D _rigidbody;
        private Vector3 _moveInput;
        private float _health;
        private float _damageReduction;
        private DamageType _reducedDamageType;
        private GameObject _equippedItemPrefab;
        private bool _canMove = true;
        private bool _canInteract = true;
        
        /// <summary>
        /// Action invoked when the player dies
        /// </summary>
        public event Action OnDeath;
        
        /// <summary>
        /// Action invoked when the player receives damage or is healed
        /// </summary>
        public event Action OnWakeUpMeterUpdated;
        
        /// <summary>
        /// Action invoked when player picks up an item
        /// </summary>
        public event Action<string> OnItemEquipped;

    
        void Start()
        {
            // Ensure only one player is in the scene at the time
            var player = GameObject.FindGameObjectWithTag(Tags.Player); 
            if (player != null && player != gameObject)
            {
                Destroy(gameObject);
            }
            DontDestroyOnLoad(this);
            _rigidbody = GetComponent<Rigidbody2D>();
            
            // Get the speed modifier from the upgrades and apply it
            speed *= GetPlayerSpeedModifier();
            // Get the sleep meter modifier from the upgrades and apply it
            maxSleepMeter *= GetSleepMeterModifier();
        }
        
        /// <summary>
        /// Method for obtaining the speed modifier from the upgrades
        /// </summary>
        /// <returns>Current speed modifier</returns>
        float GetPlayerSpeedModifier()
        {
            var speedUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Slippers);
            speedUpgrade?.ApplyEffect();
            if (speedUpgrade is PlayerSpeedUpgrade psu)
            {
                return psu.SpeedMultiplier;
            }
            return 1;
        }
        
        /// <summary>
        /// Method for obtaining sleep meter modifier from the upgrades
        /// </summary>
        /// <returns>Current sleep meter modifier</returns>
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
        
        /// <summary>
        /// Method for obtaining the damage reduction modifier from upgrades
        /// </summary>
        /// <returns>Current damage reduction</returns>
        float GetDamageReductionModifier()
        {
            var lightDamageUpgrade = UpgradeManager.Instance.GetUpgradeByKey(UpgradeKey.Pyjama);
            lightDamageUpgrade?.ApplyEffect();
            if (lightDamageUpgrade is LightDamageUpgrade ldmg)
            {
                return ldmg.LightDamageModifier;
            }
            return 1;
        }
        
        /// <summary>
        /// Method for toggling on and off whether the player can move and interact with things
        /// </summary>
        public void ToggleMovement()
        {
            _canMove = !_canMove;
            _canInteract = !_canInteract;
        }
    
        /// <summary>
        /// Method to heal (restore the wakeup meter) the player by a certain amount.
        /// </summary>
        /// <param name="healAmount">Amount to heal by</param>
        public void Heal(float healAmount)
        {
            _health -= healAmount;
            _health = Mathf.Clamp(_health, 0, maxSleepMeter);
            OnWakeUpMeterUpdated?.Invoke();
        }
        
        void FixedUpdate()
        {
            _rigidbody.velocity = speed * Time.fixedDeltaTime * _moveInput;
        }
        
        /// <summary>
        /// Method for dealing damage to the player
        /// </summary>
        /// <param name="damage">How much damage should be dealt</param>
        /// <param name="damageType">What type of damage should be dealt</param>
        public void DamagePlayer(float damage, DamageType damageType = DamageType.None)
        {
            // If player is currently invincible skip the body of the function
            if (godMode) return;
        
            float finalDamage = damage;
           
            // Apply modifier from upgrades
            Debug.Log($"[PlayerController] Current damage reduction modifier is {GetDamageReductionModifier()}");
            finalDamage *= GetDamageReductionModifier();
        
            // Account for item effect
            if (_equippedItemPrefab != null && damageType == _reducedDamageType)
            {
                finalDamage *= (1 - _damageReduction);
            }
        
            Debug.Log($"[PlayerController] Taking Damage: {finalDamage}");
            _health += finalDamage;

            if (_health > maxSleepMeter)
            {
                Debug.Log($"[PlayerController] Player is dead!");
                Die();
            }
        
            OnWakeUpMeterUpdated?.Invoke();
        }
        
        /// <summary>
        /// Method for obtaining current state of the wakeup meter (current player health)
        /// </summary>
        /// <returns></returns>
        public float GetCurrentWakeupMeter()
        {
            return _health;
        }

        public void OnMove(InputValue value)
        {
            _moveInput = _canMove ? value.Get<Vector2>() : new Vector2(0, 0);
        }
        void Update()
        {
            HandleInteraction();
        }
        
        /// <summary>
        /// Method for handling interactions with the surroundings
        /// </summary>
        private void HandleInteraction()
        {
            // Get all the objects in the area
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactionRadius);
            Interactable closestInteractable = null;
            float minDistance = Mathf.Infinity;
            
            // Find the closest object from the class Interacitble
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
            
            // If there is such an object and player can interact with it prompt an interact popup 
            if (closestInteractable != null && _canInteract)
            {
                UIManager.Instance.ToggleInteractionPopUp(true, closestInteractable,
                    callback: (() => closestInteractable.Interact(this)));
            }
            else
            {
                UIManager.Instance.ToggleInteractionPopUp(false);
            }
        }
        
        /// <summary>
        /// Method for equipping different items
        /// </summary>
        /// <param name="newItem">item to equip</param>
        public void EquipItem(Item newItem)
        {
            if (newItem == null) return;
            
            // Drop the current item
            if (_equippedItemPrefab != null)
            {
                DropCurrentItem();
            }
            
            // Updated parameters related to the current item
            OnItemEquipped?.Invoke(newItem.GetDescription());
            _damageReduction = newItem.GetDamageReduction();
            _reducedDamageType = newItem.GetAffectedDamageType();
            if (newItem is DecafCoffee)    
            {
                _equippedItemPrefab = decafCoffeePrefab;
            }
            else if (newItem is StylishShades)
            {
                _equippedItemPrefab = stylishShadesPrefab;
            }
            
            // Update the UI
            UIManager.Instance.UpdateEquippedItemUI(GetItemSprite(newItem));
            
            // Delete the picked up item from the ground
            Destroy(newItem.gameObject);
        }
        
        /// <summary>
        /// Method for dropping the current item
        /// </summary>
        void DropCurrentItem()
        {
            if (_equippedItemPrefab != null)
            {
                // Spawn the item on the ground
                Instantiate(_equippedItemPrefab, transform.position, Quaternion.identity);
                
                // Update the variables
                _equippedItemPrefab = null;
                _damageReduction = 0f;
                _reducedDamageType = default;
                
                // Update the UI
                UIManager.Instance.UpdateEquippedItemUI(null);
            }
        }
    
        /// <summary>
        /// Method to be executed when the player dies
        /// </summary>
        private void Die()
        {
            OnDeath?.Invoke();
            DropCurrentItem();
            Destroy(gameObject);
        }

        /// <summary>
        /// Method for obtaining sprite from an item
        /// </summary>
        /// <param name="item">Item to obtain the sprite from</param>
        /// <returns>Items sprite</returns>
        Sprite GetItemSprite(Item item)
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
}
