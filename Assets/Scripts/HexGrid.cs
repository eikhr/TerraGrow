using System.Collections;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexTilePrefab; // Prefab for the hex tiles
    public GameObject[] hexTilePrefabs; // Prefabs for the hex tiles
    public int size = 7; // Radius of the hexagonal island
    public HexTile[,] hexTiles;

    void Start()
    {
        hexTiles = new HexTile[size, size];
        GenerateHexagonalIsland(size);
    }

    void GenerateHexagonalIsland(int size)
    {
        for (int y = 0; y < size; y++)
        {
            int start = Mathf.Max((size - 1) / 2 - y, 0);
            int end = Mathf.Min(size - ((size - 1) / 2 - (size-y-1)), size);
            for (int x = start; x < end; x++)
            {
                CreateHexTile(x, y);
            }
        }
    }

    void CreateHexTile(int x, int y)
    {
        GameObject hexTile = Instantiate(hexTilePrefabs[Random.Range(0, hexTilePrefabs.Length)], Vector3.zero, Quaternion.identity);
        hexTile.transform.SetParent(transform); // Parent it to the grid for organization
        
        HexTile hex = hexTile.GetComponent<HexTile>();
        if (hex != null)
        {
            hex.Initialize(x, y);
        }
        hexTiles[x, y] = hex;
    }

    public void SetTile(int x, int y, HexTile.TileType tileType) {
        GameObject hexTile = Instantiate(hexTilePrefabs[(int)tileType], Vector3.zero, Quaternion.identity);
        hexTile.transform.SetParent(transform); // Parent it to the grid for organization
        
        // Destroy the old tile
        ReplaceTile(x, y, hexTile);
    }
    
    public HexTile GetHexTile(int x, int y)
    {
        if (x < 0 || x >= size || y < 0 || y >= size)
        {
            return null;
        }
        return hexTiles[x, y];
    }
    
    public HexTile[] GetNeighbours(int x, int y)
    {
        HexTile[] neighbours = new HexTile[6];
        neighbours[0] = GetHexTile(x - 1, y + 1); // North West
        neighbours[1] = GetHexTile(x, y + 1); // North East
        neighbours[2] = GetHexTile(x - 1, y); // West
        neighbours[3] = GetHexTile(x + 1, y); // East
        neighbours[4] = GetHexTile(x, y - 1); // South West
        neighbours[5] = GetHexTile(x + 1, y - 1); // South East
        return neighbours;
    }
    
    public void ReplaceTile(int x, int y, GameObject newTile)
    {
        var hex = newTile.GetComponent<HexTile>();
        if (hex != null)
        {
            hex.Initialize(x, y);
            newTile.transform.position = hexTiles[x, y].transform.position;
        }
        StartCoroutine(ReplaceTileTransition(hexTiles[x, y].gameObject, newTile));
        hexTiles[x, y] = hex;
    }
    
    private IEnumerator ReplaceTileTransition(GameObject oldTile, GameObject newTile)
    {
        // Flip over the tiles
        for (float t = 0; t <= 1f; t += Time.deltaTime * 1.3f)
        {
            // Rotate along the x-axis
            oldTile.transform.rotation = Quaternion.Lerp(Quaternion.identity, new Quaternion(1, 0, 0, 0), t );
            newTile.transform.rotation = Quaternion.Lerp(new Quaternion(-1, 0, 0, 0), Quaternion.identity, t);
            yield return null;
        }

        // Destroy the old tile
        Destroy(oldTile);
    }
}