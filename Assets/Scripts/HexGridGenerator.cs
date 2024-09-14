using UnityEngine;

public class HexGridGenerator : MonoBehaviour
{
    public GameObject hexTilePrefab; // Prefab for the hex tiles
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
        GameObject hexTile = Instantiate(hexTilePrefab, Vector3.zero, Quaternion.identity);
        hexTile.transform.SetParent(transform); // Parent it to the grid for organization
        
        HexTile hex = hexTile.GetComponent<HexTile>();
        if (hex != null)
        {
            hex.Initialize(x, y);
        }
        hexTiles[x, y] = hex;
    }
}