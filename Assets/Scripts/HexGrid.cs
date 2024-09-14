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

        HexTile hex = hexTile.GetComponent<HexTile>();
        if (hex != null)
        {
            hex.Initialize(x, y);
        }
        
        // Destroy the old tile
        Destroy(hexTiles[x, y].gameObject);

        hexTiles[x, y] = hex;
        
    }
}