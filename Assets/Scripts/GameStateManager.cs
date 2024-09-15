using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public int turn = 0;
    public int playerScore = 0;
    public int nextGoodieBagScore = 2;
    
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
                                listHasTiles(neighbours, HexTile.TileType.Mountain, 1) &&
                                listHasTiles(neighbours, HexTile.TileType.Water, 1))
                            {
                                tile.LevelUp(HexTile.TileType.BigMushroom);
                            }

                            break;

                        case HexTile.TileType.Animals:
                            // Rule: If next to Mushroom, downgrade to Grass
                            if (listHasTiles(neighbours, HexTile.TileType.Mushroom, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Grass);
                            }
                            // Rule: If adjacent to Village and not adjacent to Mushroom, become Animal Pen
                            else if (listHasTiles(neighbours, HexTile.TileType.Village, 1) &&
                                     !listHasTiles(neighbours, HexTile.TileType.Mushroom, 1))
                            {
                                tile.LevelUp(HexTile.TileType.AnimalPen);
                            }

                            break;

                        case HexTile.TileType.Grass:
                            // Rule: If adjacent to two Forests, become Forest
                            if (listHasTiles(neighbours, HexTile.TileType.Forest, 2))
                            {
                                tile.LevelUp(HexTile.TileType.Forest);
                            }
                            // Rule: If adjacent to Water and Flowers, become Flowers
                            else if (listHasTiles(neighbours, HexTile.TileType.Water, 1) &&
                                     listHasTiles(neighbours, HexTile.TileType.Flowers, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Flowers);
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

                        case HexTile.TileType.Flowers:
                            // Rule: If adjacent to Village, become Crops
                            if (listHasTiles(neighbours, HexTile.TileType.Village, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Crops);
                            }

                            break;

                        case HexTile.TileType.Crops:
                            // Rule: If not adjacent to Water, become Grass
                            if (!listHasTiles(neighbours, HexTile.TileType.Water, 1))
                            {
                                tile.LevelUp(HexTile.TileType.Grass);
                            }

                            break;

                        // Add other tile types as needed
                    }
                }
            }
        }
    }

    private bool listHasTiles(List<HexTile> tiles, HexTile.TileType tileType, int count)
    {
        int tileCount = 0;
        foreach (HexTile tile in tiles)
        {
            if (tile.tileType == tileType)
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