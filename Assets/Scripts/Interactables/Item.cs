using Gameplay;
using UnityEngine;
using Utility;

namespace Interactables
{
    /// <summary>
    /// Abstract class for items
    /// </summary>
    public abstract class Item: Interactable
    {
        [Tooltip("Item sprite of the item")]
        [SerializeField] private Sprite itemSprite;
        
        [Tooltip("How much should the item reduce the damage taken")]
        [SerializeField] private float damageReduction = 0.3f;
        
        [Tooltip("What type of damage should be affected by the item")]
        [SerializeField] private DamageType affectedDamageType = DamageType.Fire;
        
        /// <summary>
        /// Override for the <c>Interact</c> method
        /// </summary>
        /// <param name="player">PlayerController reference</param>
        public override void Interact(PlayerController player)
        {
            player.EquipItem(this);
        }
        
        /// <summary>
        /// Method for getting the item sprite
        /// </summary>
        /// <returns>Item sprite</returns>
        public Sprite GetItemSprite()
        {
            return itemSprite;
        }
        
        /// <summary>
        /// Method for getting the items damage reduction
        /// </summary>
        /// <returns>Item damage reduction</returns>
        public float GetDamageReduction()
        {
            return damageReduction;
        }
        
        /// <summary>
        /// Method for getting the items affected damage type
        /// </summary>
        /// <returns>Items affected damage type</returns>
        public DamageType GetAffectedDamageType()
        {
            return affectedDamageType;
        }
        
        /// <summary>
        /// Abstract method for getting items description
        /// </summary>
        /// <returns>Item description</returns>
        public abstract string GetDescription();
    }
}