using UnityEngine;
using Utility;

namespace Triggers
{
    /// <summary>
    /// Class for spawning the player when he enter the trigger
    /// </summary>
    public class ScreamerTrigger : MonoBehaviour
    {
        [Tooltip("Screamer prefab")]
        [SerializeField] private GameObject screamerPrefab;
        
        [Tooltip("Object where to spawn the screamer")]
        [SerializeField] private Transform spawnObject;

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag(Tags.Player))
            {
                // Dont spawn the screamer if he already exists
                var existingScreamer = GameObject.FindGameObjectWithTag(Tags.Screamer);
                if (existingScreamer == null)
                {
                    var screamer = Instantiate(screamerPrefab, spawnObject);
                    screamer.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }
}
