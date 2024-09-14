using System.Collections;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    const int DEFAULT_TIMER = 4;  
    public enum TileType
    {
        Grass, // Default, no special properties
        Water, // Lake
        Mountain,
        Forest,
        Mushroom, // cut trees
        Flowers,
        Crops,
        Animals,
        Town,
    }
    public int x;  // Grid coordinates
    public int y;
    
    public int basePointRewards = 1;
    
    public TileType tileType;
    public int tileLevel = 1; 
    public GameObject nextLevelPrefab;
    
    public int turnsUntilLevelUp = DEFAULT_TIMER;
    public int wantsWater = 0;
    public int wantsMinerals = 0;
    public int wantsShade = 0;
    public int wantsSunlight = 0;
    public int givesWater = 0;
    public int givesMinerals = 0;
    public int givesShade = 0;
    public int givesSunlight = 0;

    public Vector3 Position => OffsetToWorldPosition(x, y);

    // Convert offset coordinates (x, y) to world position
    public static Vector3 OffsetToWorldPosition(int x, int y)
    {
        float width = Mathf.Sqrt(3); // Width of each hex tile
        float height = 2; // Height of each hex tile

        float xOffset = width * (x + 0.5f * y); // Apply offset for odd or even rows
        float zOffset = height * (3f / 4f * y);

        return new Vector3(xOffset, 0, zOffset);
    }

    public void Initialize(int x, int y)
    {
        this.x = x;
        this.y = y;
        transform.position = Position; // Set the position based on offset coordinates
    }

    public void Select()
    {
        StartCoroutine(AnimateSelection(new Vector3(transform.position.x, 0.5f, transform.position.z)));
    }

    public void Unselect()
    {
        StartCoroutine(AnimateSelection(new Vector3(transform.position.x, 0, transform.position.z)));
    }

    private IEnumerator AnimateSelection(Vector3 targetPosition)
    {
        Vector3 startPosition = transform.position;
        float duration = 0.1f; // Duration of the animation
        float elapsed = 0f;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition; // Ensure the final position is set
    }
}