using System.Collections;
using System.Collections.Generic;
using System;

using UnityEngine;
using UnityEngine.Tilemaps;

public class HexTileClickHandler : MonoBehaviour
{
    public Tilemap tilemap; // Assign the tilemap in the Inspector
    void Start() {
        Debug.Log("Startup");
        tilemap = GetComponent<Tilemap>();
    }

    void Wake() {
    }

    void Update()
    {
        // Detect click
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int cellPosition = tilemap.WorldToCell(worldPos);
    
        
            Debug.Log(worldPos);
            Debug.Log(cellPosition);
    
            // Check if tile exists
            if (tilemap.HasTile(cellPosition))
            {
                Console.WriteLine("Hello");
                Debug.Log("Test");
                AffectSurroundingTiles(cellPosition);
            }
        }
    }

    // Method to affect surrounding tiles
    void AffectSurroundingTiles(Vector3Int centerTilePos)
    {
        // Hexagonal tiles have six neighbors. Get each of them.
        Vector3Int[] neighborOffsets = new Vector3Int[]
        {
            new Vector3Int(1, 0, 0),    // Right
            new Vector3Int(0, 1, 0),    // Top-right
            new Vector3Int(-1, 1, 0),   // Top-left
            new Vector3Int(-1, 0, 0),   // Left
            new Vector3Int(0, -1, 0),   // Bottom-left
            new Vector3Int(1, -1, 0)    // Bottom-right
        };

        foreach (var offset in neighborOffsets)
        {
            Vector3Int neighborPos = centerTilePos + offset;
            if (tilemap.HasTile(neighborPos))
            {
                // You can add logic to change the tile at the neighbor position
                ChangeTile(neighborPos);
            }
        }
    }

    // Example of how you can change or affect a tile
    void ChangeTile(Vector3Int tilePos)
    {
        // This can be replaced by any logic, like changing tile color, removing, etc.
        // Example: Changing the color of the tile
        Tile tile = tilemap.GetTile<Tile>(tilePos);
        if (tile != null)
        {
            // Assuming you're using a Tile that supports color tinting.
            tilemap.SetTileFlags(tilePos, TileFlags.None);
            tilemap.SetColor(tilePos, Color.red); // Change the color of the affected tile
        }
    }
}
