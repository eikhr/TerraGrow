using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public int selectedX = 3;
    public int selectedY = 0;

    private HexGridGenerator _generator;
    
    void Start()
    {
        _generator = GetComponent<HexGridGenerator>();
    }
    
    void ChooseTile(int x, int y)
    {
        if (x > _generator.size - 1 || x < 0 || y > _generator.size - 1 || y < 0)
        {
            return;
        }
        HexTile selectedTile = _generator.hexTiles[x, y];
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
        HexTile selectedTile = _generator.hexTiles[x, y];
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
