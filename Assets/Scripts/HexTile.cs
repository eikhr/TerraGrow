using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Village,
        Factory,
        BigMushroom,
    }
    public enum TileResource
    {
        Water,
        Minerals,
        Shade,
        Sunlight,
    }
    public int x;  // Grid coordinates
    public int y;
    
    public int basePointRewards = 1;
    
    public TileType tileType;
    public int tileLevel = 1; 
    public GameObject nextLevelPrefab;
    
    public int turnsUntilLevelUp = DEFAULT_TIMER;
    public Dictionary<TileResource, int> ResourceRequirements = new Dictionary<TileResource, int>();
    public Dictionary<TileResource, int> ResourceRewards = new Dictionary<TileResource, int>();
    
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
    
    public HexGrid GetGrid()
    {
        return GetComponentInParent<HexGrid>();
    }

    public bool HasResourcesCovered()
    {
        var neighbours = GetGrid().GetNeighbours(x, y);
        Dictionary<TileResource, int> uncoveredResources = new Dictionary<TileResource, int>();
        foreach (var resource in ResourceRequirements)
        {
            uncoveredResources[resource.Key] = resource.Value;
        }
        foreach (var neighbour in neighbours)
        {
            if (neighbour != null)
            {
                foreach (var resource in ResourceRequirements)
                {
                    uncoveredResources[resource.Key] -= neighbour.ResourceRewards[resource.Key];
                }
            }
        }
        return uncoveredResources.Values.All(value => value <= 0);
    }
    

    public void LevelUp(GameObject newTilePrefab, TileType newTileType)
    {
        if (nextLevelPrefab != null)
        {
            GameObject newTile = Instantiate(newTilePrefab, transform.position, Quaternion.identity);
            HexTile newHexTile = newTile.GetComponent<HexTile>();
            newHexTile.Initialize(x, y);
            newHexTile.tileType = newTileType;
            GetGrid().ReplaceTile(x, y, newTile);
        }
    }
    
}