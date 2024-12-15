using UnityEngine;

public class ScreamerSpawner : MonoBehaviour
{
    [SerializeField] private GameObject screamerPrefab;
    [SerializeField] private float yOffsetFromHallway = 50f;

    void Start()
    {
        SpawnScreamer();
    }

    void SpawnScreamer()
    {
        GenerateHallway hallwayGenerator = GetComponent<GenerateHallway>();
        if (hallwayGenerator == null)
        {
            Debug.LogError("GenerateHallway component not found on the hallway GameObject.");
            return;
        }

        float hallwayWidth = hallwayGenerator.HallwayWidth;
        float hallwayBottomY = -hallwayGenerator.HallwayLength / 2;

        float xPosition = 0;
        float yPosition = hallwayBottomY + yOffsetFromHallway / 100f;

        GameObject newScreamer = Instantiate(screamerPrefab, transform);
        newScreamer.transform.localPosition = new Vector3(xPosition, yPosition, 0);

        Debug.Log("Screamer spawned at position: " + newScreamer.transform.localPosition);
    }
}
