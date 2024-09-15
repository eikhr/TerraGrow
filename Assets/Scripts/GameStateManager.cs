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