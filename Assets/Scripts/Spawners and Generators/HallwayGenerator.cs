using Managers;
using UnityEngine;

namespace Spawners_and_Generators
{
    public class GenerateHallway : MonoBehaviour
    {
        [SerializeField] GameObject hallwayStart;
        [SerializeField] SpriteRenderer bedSprite;
        [SerializeField] GameObject hallwayEnd;
        [SerializeField] private float minLength = 50;
        [SerializeField] private float maxLength = 100;
        [SerializeField] private float spawnableWidthOffset = 8;

        public float HallwayWidth {get; private set;}
        public float HallwayLength {get; private set;}
        public float HallwayCameraLength {get; private set;}
    
        void Start()
        {
            minLength *= DifficultyManager.Instance.HallwayLengthCoeficient;
            maxLength *= DifficultyManager.Instance.HallwayLengthCoeficient;
            CreateHallway();
        }

        void CreateHallway()
        {
            float hallwayStartLength = hallwayStart.GetComponent<SpriteRenderer>().size.y;
            float hallwayEndLength = hallwayEnd.GetComponent<SpriteRenderer>().size.y;
            GameObject floor = transform.Find("Floor").gameObject;
            HallwayWidth = floor.GetComponent<SpriteRenderer>().size.x;
            HallwayLength = Random.Range(minLength, maxLength);
            HallwayCameraLength = HallwayLength+hallwayStartLength+hallwayEndLength;
            floor.GetComponent<SpriteRenderer>().size = new Vector2(HallwayWidth, HallwayLength);


            Vector2[] points = 
            {
                new (-HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
                new (HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
                new (HallwayWidth/2, HallwayLength/2+hallwayStartLength),
                new (-HallwayWidth/2, HallwayLength/2+hallwayStartLength),
                new (-HallwayWidth/2, -HallwayLength/2-hallwayStartLength),
            };
            floor.GetComponent<EdgeCollider2D>().points = points;
            transform.position = new Vector3(0, HallwayLength/2, 0);
            hallwayEnd.transform.localPosition= new Vector3(0, HallwayLength/2+hallwayEndLength/2, 0);
            hallwayStart.transform.localPosition = new Vector3(0, -HallwayLength/2-hallwayStartLength/2, 0);

            HallwayWidth -= spawnableWidthOffset;
        
            if (DifficultyManager.Instance.currentDifficulty > 1)
            {
                bedSprite.enabled = false;
            }
        }
    }
}
