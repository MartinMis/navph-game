using Controllers;
using UnityEngine;
using Utility;

namespace Items_and_Consumables
{
    public class DecafCoffee : Interactable
    {
        [SerializeField] private Sprite itemSprite;
        [SerializeField] private float damageReduction = 0.3f;
        [SerializeField] private DamageType affectedDamageType = DamageType.Coffee;

        public override void Interact(PlayerController player)
        {
            player.EquipItem(this);
        }

        public Sprite GetItemSprite()
        {
            return itemSprite;
        }

        public float GetDamageReduction()
        {
            return damageReduction;
        }
    
        public DamageType GetAffectedDamageType()
        {
            return affectedDamageType;
        }
    }
}
