using Managers;
using UnityEngine;

namespace Spawners
{
    /// <summary>
    /// Class for generating the hallways
    /// </summary>
    public class GenerateHallway : MonoBehaviour
    {
        [Tooltip("Prefab for the start of the hallway")]
        [SerializeField] GameObject hallwayStart;
        
        [Tooltip("Sprite of the bed at the start")]
        [SerializeField] SpriteRenderer bedSprite;
        
        [Tooltip("Prefab of the end of the hallway")]
        [SerializeField] GameObject hallwayEnd;
        
        [Tooltip("Minimal possible length of the hallway")]
        [SerializeField] private float minLength = 50;
        
        [Tooltip("Maximal possible length of the hallway")]
        [SerializeField] private float maxLength = 100;
        
        [Tooltip("Offset of the spawnable area")]
        [SerializeField] private float spawnableWidthOffset = 8;

        public float HallwayWidth {get; private set;}
        public float HallwayLength {get; private set;}
        public float HallwayCameraLength {get; private set;}
    
        void Start()
        {
            // Adjust length with difficulty coefficients
            minLength *= DifficultyManager.Instance.HallwayLengthCoeficient;
            maxLength *= DifficultyManager.Instance.HallwayLengthCoeficient;
            CreateHallway();
        }

        void CreateHallway()
        {
            float hallwayStartLength = hallwayStart.GetComponent<SpriteRenderer>().size.y;
            float hallwayEndLength = hallwayEnd.GetComponent<SpriteRenderer>().size.y;
            GameObject floor = transform.Find("Floor").gameObject;
            // Get size of the hallway
            HallwayWidth = floor.GetComponent<SpriteRenderer>().size.x;
            HallwayLength = Random.Range(minLength, maxLength);
            // Calculate the lenght of the hallway for the camera
            HallwayCameraLength = HallwayLength+hallwayStartLength+hallwayEndLength;
            // Adjust the sprite to the new size
            floor.GetComponent<SpriteRenderer>().size = new Vector2(HallwayWidth, HallwayLength);
            // Generate points for the collider and assign them
            Vector2[] points = 
            {
                new (-HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
                new (HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
                new (HallwayWidth/2, HallwayLength/2+hallwayStartLength),
                new (-HallwayWidth/2, HallwayLength/2+hallwayStartLength),
                new (-HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
            };
            floor.GetComponent<EdgeCollider2D>().points = points;
            // Move the hallway so the player doesnt start in the middle
            transform.position = new Vector3(0, HallwayLength/2, 0);
            hallwayEnd.transform.localPosition= new Vector3(0, HallwayLength/2+hallwayEndLength/2, 0);
            hallwayStart.transform.localPosition = new Vector3(0, -HallwayLength/2-hallwayStartLength/2, 0);

            HallwayWidth -= spawnableWidthOffset;
            
            // If level is higher than one do not show the starting bed
            if (DifficultyManager.Instance.CurrentDifficulty > 1)
            {
                bedSprite.enabled = false;
            }
        }
    }
}
