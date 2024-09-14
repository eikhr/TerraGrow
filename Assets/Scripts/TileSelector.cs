using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public int selectedX = 3;
    public int selectedY = 0;

    private HexGrid _grid;
    
    void Start()
    {
        _grid = GetComponent<HexGrid>();
    }
    
    void ChooseTile(int x, int y)
    {
        if (x > _grid.size - 1 || x < 0 || y > _grid.size - 1 || y < 0)
        {
            return;
        }
        HexTile selectedTile = _grid.hexTiles[x, y];
        if (selectedTile == null)
        {
            return;
        }
        UnchooseTile(selectedX, selectedY);
        selectedX = x;
        selectedY = y;
        selectedTile.Select();
    }   
    
    void UnchooseTile(int x, int y)
    {
        HexTile selectedTile = _grid.hexTiles[x, y];
        if (selectedTile != null)
        {
            selectedTile.Unselect();
        }
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            ChooseTile(selectedX, selectedY + 1);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ChooseTile(selectedX, selectedY - 1);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            ChooseTile(selectedX - 1, selectedY);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ChooseTile(selectedX + 1, selectedY);
        }
    }
}
