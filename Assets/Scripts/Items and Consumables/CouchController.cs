using System.Collections;
using Controllers;
using Unity.VisualScripting;
using UnityEngine;

namespace Items_and_Consumables
{
    public class CouchController : Interactable
    {
        [SerializeField] private float restTime = 10f;
        [SerializeField] private float reductionAmount = 50;

        public override void Interact(PlayerController player)
        {
            var originalPlayerPosition = player.transform.position;
            player.ToggleMovement();
            player.GameObject().GetComponent<CapsuleCollider2D>().enabled = false;
            player.transform.position = transform.position;
            player.GodMode = true;
            Debug.Log("[CouchController] Interact");
            StartCoroutine(PerformRest(player, restTime, reductionAmount, originalPlayerPosition));
        }

        IEnumerator PerformRest(PlayerController player, float timeRequired, float restoreAmount, Vector3 restorePosition)
        {
            Debug.Log($"[CouchController] Performing rest for {timeRequired} seconds");
            yield return new WaitForSeconds(timeRequired);
            player.transform.position = restorePosition;
            player.GameObject().GetComponent<CapsuleCollider2D>().enabled = true;
            player.Heal(restoreAmount);
            player.GodMode = false;
            player.ToggleMovement();
        }
    }
}
