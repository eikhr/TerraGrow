using System.Collections;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public GameObject hexTilePrefab; // Prefab for the hex tiles
    public GameObject[] hexTilePrefabs; // Prefabs for the hex tiles
    public int size = 7; // Radius of the hexagonal island
    private HexTile[,] _hexTiles;

    void Start()
    {
        _hexTiles = new HexTile[size, size];
        GenerateHexagonalIsland(size);
    }
    
    private void GenerateHexagonalIsland(int size)
    {
        for (int y = 0; y < size; y++)
        {
            int start = Mathf.Max((size - 1) / 2 - y, 0);
            int end = Mathf.Min(size - ((size - 1) / 2 - (size - y - 1)), size);
            for (int x = start; x < end; x++)
            {
                CreateInitialGrassHexTile(x, y);
            }
        }
        StartCoroutine(GenerateHexagonalIslandCoroutine(size));
    }

    private IEnumerator GenerateHexagonalIslandCoroutine(int size)
    {
        for (int y = 0; y < size; y++)
        {
            int start = Mathf.Max((size - 1) / 2 - y, 0);
            int end = Mathf.Min(size - ((size - 1) / 2 - (size - y - 1)), size);
            for (int x = start; x < end; x++)
            {
                if (CreateRandomHexTile(x, y))
                {
                    yield return new WaitForSeconds(0.02f); // Adjust the delay as needed
                }
            }
        }
    }
    
    void CreateInitialGrassHexTile(int x, int y)
    {
        GameObject hexTile = Instantiate(hexTilePrefabs[(int)HexTile.TileType.Grass], Vector3.zero, Quaternion.identity);
        hexTile.transform.SetParent(transform); // Parent it to the grid for organization
        HexTile hex = hexTile.GetComponent<HexTile>();
        hex.Initialize(x, y);
        _hexTiles[x, y] = hex;
    }

    bool CreateRandomHexTile(int x, int y)
    {
        // Randomly set the tile type and replace the grass tile
        var random = Random.Range(0, 100);
        if (random < 10)
        {
            SetTile(x, y, HexTile.TileType.Mountain);
        }
        else if (random < 20)
        {
            SetTile(x, y, HexTile.TileType.Water);
        }
        else
        {
            return false;
        }

        return true;
    }

    public void SetTile(int x, int y, HexTile.TileType tileType) {
        GameObject hexTile = Instantiate(hexTilePrefabs[(int)tileType], Vector3.zero, Quaternion.identity);
        hexTile.transform.SetParent(transform); // Parent it to the grid for organization
        
        ReplaceTile(x, y, hexTile);
    }
    
    public HexTile GetHexTile(int x, int y)
    {
        if (x < 0 || x >= size || y < 0 || y >= size)
        {
            return null;
        }
        return _hexTiles[x, y];
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
            newTile.transform.position = _hexTiles[x, y].transform.position;
        }
        
        StartCoroutine(ReplaceTileTransition(_hexTiles[x, y].gameObject, newTile));
        _hexTiles[x, y] = hex;
    }
    
    private IEnumerator ReplaceTileTransition(GameObject oldTile, GameObject newTile)
    {
        // Flip over the tiles
        for (float t = 0; t <= 1f; t += Time.deltaTime * 1.3f)
        {
            // Always keep the tiles at the same position
            oldTile.transform.position = newTile.transform.position;
            // Rotate along the x-axis
            oldTile.transform.rotation = Quaternion.Lerp(Quaternion.identity, new Quaternion(1, 0, 0, 0), t );
            newTile.transform.rotation = Quaternion.Lerp(new Quaternion(-1, 0, 0, 0), Quaternion.identity, t);
            yield return null;
        }

        // Destroy the old tile
        Destroy(oldTile);
    }
}