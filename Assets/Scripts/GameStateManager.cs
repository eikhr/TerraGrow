using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int turn = 0;
    public int playerScore = 0;
    public int nextGoodieBagScore = 2;

    private int _selectedX = 0;
    private int _selectedY = 0;

    private CameraController _cameraController;
    private HexGrid _hexGrid;

    void Start()
    {
        _cameraController = FindObjectOfType<CameraController>();
        _hexGrid = FindObjectOfType<HexGrid>();

        _cameraController.DebugSomeShit();
    }

    void Update()
    {
        // End turn on enter key press
        if (Input.GetKeyDown(KeyCode.Return))
        {
            EndTurn();
        }
    }
    
    public bool PlaceTile(GameObject tileToSwap)
    {
        var selectedTile = _hexGrid.GetHexTile(_selectedX, _selectedY);
        if (selectedTile == null || selectedTile.tileType != HexTile.TileType.Grass)
        {
            return false;
        }
        var hexTile = tileToSwap.GetComponent<HexTile>();
        _hexGrid.SetTile(_selectedX, _selectedY, hexTile.tileType);
        return true;
    }
    
    public void SelectTile(int x, int y)
    {
        _selectedX = x;
        _selectedY = y;
    }
    
    public void EndTurn()
    {
        turn++;
        updateScore();
        levelUpTiles();
        tryGiveGoodieBag();
        Debug.Log("End of turn " + turn);
    }
    
    public void updateScore()
    {
        playerScore += 1;
        //TODO Implement

        //On Z button press, giveGoodieBag();
        
    }

    public void levelUpTiles()
    {
        //TODO Implement
        // 1 for each tile in the grid
        // 2 find the neighbours of the tile
        // 3 if succeeding the conditions, swap the tile with the upgrade
        
        Debug.Log("Leveling up tiles");
        Debug.Log("Player score: " + playerScore);

        for (int x = 0; x < _hexGrid.size; x++)
        {
            for (int y = 0; y < _hexGrid.size; y++)
            {
                HexTile tile = _hexGrid.GetHexTile(x, y);
                if (tile != null)
                {
                    HexTile[] neighbours = _hexGrid.GetNeighbours(x, y);
                    switch (tile.tileType)
                    {
                        case HexTile.TileType.Mushroom:
                            // Rule: If adjacent to Forest + Mountain + Water, grow into Big Mushroom
                            if (listHasTiles(neighbours, HexTile.TileType.Forest, 1) &&
                                listHasTiles(neighbours, HexTile.TileType.Mountain, 1))
                            {
                                tile.LevelUp(HexTile.TileType.BigMushroom);
                            }

                            playerScore += 30;

                            break;

                        case HexTile.TileType.Animals:
                            // Rule: If next to Mushroom, downgrade to Grass
                            if (listHasTiles(neighbours, HexTile.TileType.Mushroom, 1))
                            {
                                tile.Die();
                                playerScore -= 50;
                            }
                            // Rule: If adjacent to Village and not adjacent to Mushroom, become Animal Pen
                            else if (listHasTiles(neighbours, HexTile.TileType.Village, 1) &&
                                     !listHasTiles(neighbours, HexTile.TileType.Mushroom, 1))
                            {
                                tile.LevelUp(HexTile.TileType.AnimalPen);
                                playerScore += 20;
                            }

                            break;

                        case HexTile.TileType.Grass:
                            // Rule: If adjacent to two Forests, become Forest
                            if (listHasTiles(neighbours, HexTile.TileType.Forest, 2))
                            {
                                tile.LevelUp(HexTile.TileType.Forest);
                                playerScore += 20;
                            }
                            // Rule: If adjacent to Water and Flowers, become Flowers
                            else if (listHasTiles(neighbours, HexTile.TileType.Water, 1) &&
                                     listHasTiles(neighbours, HexTile.TileType.Flowers, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Flowers);
                                playerScore += 5;
                            }
                            // Rule: If adjacent to Water and Trees or Forest, become Trees
                            else if (listHasTiles(neighbours, HexTile.TileType.Water, 1) &&
                                     (listHasTiles(neighbours, HexTile.TileType.Trees, 1) ||
                                      listHasTiles(neighbours, HexTile.TileType.Forest, 1)))
                            {
                                tile.LevelUp(HexTile.TileType.Trees);
                                playerScore += 10;
                            }
                            // Rule: If adjacent to Water, flowers and Animals, become Animals
                            else if (listHasTiles(neighbours, HexTile.TileType.Water, 1) &&
                                     listHasTiles(neighbours, HexTile.TileType.Flowers, 1) &&
                                     listHasTiles(neighbours, HexTile.TileType.Animals, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Animals);
                                playerScore += 30;
                            }
                            // Rule: If adjacent to (MushroomBig or Mushroom) and (Forest or Mountain), become Mushroom
                            else if ((listHasTiles(neighbours, HexTile.TileType.BigMushroom, 1) ||
                                      listHasTiles(neighbours, HexTile.TileType.Mushroom, 1)) &&
                                     (listHasTiles(neighbours, HexTile.TileType.Forest, 1) ||
                                      listHasTiles(neighbours, HexTile.TileType.Mountain, 1)))
                            {
                                tile.LevelUp(HexTile.TileType.Mushroom);
                                playerScore += 25;
                            }
                            
                            break;

                        case HexTile.TileType.Village:
                            // Rule: If adjacent to Water and either Animals or Crops, become Town
                            if (listHasTiles(neighbours, HexTile.TileType.Water, 1) &&
                                (listHasTiles(neighbours, HexTile.TileType.Animals, 1) ||
                                 listHasTiles(neighbours, HexTile.TileType.Crops, 1)))
                            {
                                tile.LevelUp(HexTile.TileType.Town);
                            }
                            
                            break;
                        
                        case HexTile.TileType.Town:
                            // Rule: If adjacent to Forest and Mountain, become Factory
                            if (listHasTiles(neighbours, HexTile.TileType.Forest, 1) &&
                                listHasTiles(neighbours, HexTile.TileType.Mountain, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Factory);
                                playerScore += 50;
                            }
                            
                            break;

                        case HexTile.TileType.Flowers:
                            // Rule: If adjacent to Village, become Crops
                            if (listHasTiles(neighbours, HexTile.TileType.Village, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Crops);
                                playerScore += 40;
                            }

                            break;

                        case HexTile.TileType.Crops:
                            // Rule: If not adjacent to Water, become Grass
                            if (!listHasTiles(neighbours, HexTile.TileType.Water, 1))
                            {
                                tile.Die();
                                playerScore -= 50;
                            }

                            break;
                        
                        case HexTile.TileType.Trees:
                            // Rule: If adjacent to two trees or forest, become forest
                            if (listHasTiles(neighbours, HexTile.TileType.Trees, 2) ||
                                listHasTiles(neighbours, HexTile.TileType.Forest, 2) ||
                                listHasTiles(neighbours, HexTile.TileType.Trees, 1) &&
                                listHasTiles(neighbours, HexTile.TileType.Forest, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Forest);
                                playerScore += 20;
                            }
                            
                            break;

                        // Add other tile types as needed
                    }
                }
            }
        }

        for (int x = 0; x < _hexGrid.size; x++)
        {
            for (int y = 0; y < _hexGrid.size; y++)
            {
                HexTile tile = _hexGrid.GetHexTile(x, y);
                if (tile != null)
                {
                    tile.updated = false;
                }
            }
        }
    }

    private bool listHasTiles(HexTile[] tiles, HexTile.TileType tileType, int count)
    {
        int tileCount = 0;
        foreach (HexTile tile in tiles)
        {
            if (tile != null && !tile.updated && tile.tileType != null && tile.tileType == tileType)
            {
                tileCount++;
            }
        }
        return tileCount >= count;
    }
    
    public void tryGiveGoodieBag()
    {
        if (playerScore >= nextGoodieBagScore)
        {
            //giveGoodieBag(); // Maybe pass score as param
            
            // Calculate next goodie bag score
            nextGoodieBagScore = (nextGoodieBagScore + 2) * 2;
        }
    }

}