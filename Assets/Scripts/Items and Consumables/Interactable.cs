using UnityEngine;

public abstract class Interactable : MonoBehaviour
{
    public float interactionRadius = 3;

    public abstract void Interact(PlayerController player);
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}