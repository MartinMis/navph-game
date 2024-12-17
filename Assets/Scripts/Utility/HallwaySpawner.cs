using UnityEngine;

public class HallwaySpawner
{
    public static Vector3 SpawnPosition (float width, float height, float widthPadding, float heightPadding)
    {
        float xPosition = Random.Range((-width+widthPadding)/2, (width-widthPadding)/2);
        float yPosition = Random.Range((-height+heightPadding)/2, (height-heightPadding)/2);
        
        return new Vector3(xPosition, yPosition, 0);
    }
}
