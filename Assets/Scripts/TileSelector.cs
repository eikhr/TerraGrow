using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public int selectedX = 3;
    public int selectedY = 0;
    
    private HexGrid _grid;

    public GameStateManager gameStateManager;
    
    private readonly Vector3 _cameraOffset = new Vector3(0, 10, -10);

    void Start()
    {
        _grid = GetComponent<HexGrid>();

        selectedX = _grid.size / 2;
        selectedY = _grid.size / 2;

        // Initialize the camera position
        Camera targetCamera = Camera.main;
        
        HexTile selectedTile = _grid.GetHexTile(selectedX, selectedY);
        targetCamera.transform.position = selectedTile.Position + _cameraOffset;
    }


    void ChooseTile(int x, int y)
    { 
        gameStateManager.SelectTile(x, y);
        
        HexTile selectedTile = _grid.GetHexTile(x, y);
        if (selectedTile == null) return;
        
        UnchooseTile(selectedX, selectedY);
        selectedX = x;
        selectedY = y;
        selectedTile.Select();
        
        // Find new camera position
        Vector3 position = selectedTile.Position + _cameraOffset;
        var cameraController = Camera.main?.GetComponent<CameraController>();
        cameraController?.MoveTowards(position);
       
    } 
    
    void UnchooseTile(int x, int y)
    {
        HexTile selectedTile = _grid.GetHexTile(x, y);
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
        else if (Input.GetKeyDown(KeyCode.Space))
        {
            HexTile selectedTile = _grid.GetHexTile(selectedX, selectedY);
            _grid.SetTile(selectedX, selectedY, selectedTile.tileType);

            Debug.Log("Space pressed");
            gameStateManager.EndTurn();
        }
    }
}
