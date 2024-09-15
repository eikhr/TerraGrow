using UnityEngine;

public class TileSelector : MonoBehaviour
{
    public int selectedX = 3;
    public int selectedY = 0;

    public float cameraShiftDuration = 0.15f; // Duration of the animation when shifting the camera between tiles

    public HexTile.TileType selectedTileType = HexTile.TileType.Mountain;

    private HexGrid _grid;

    public GameStateManager gameStateManager;

    void Start()
    {
        _grid = GetComponent<HexGrid>();

        selectedX = _grid.size / 2;
        selectedY = _grid.size / 2;

        // Initialize the camera position
        Camera targetCamera = Camera.main;

        Vector3 offsetFromTile = new Vector3(0, 10, -10);

        HexTile selectedTile = _grid.GetHexTile(selectedX, selectedY);
        targetCamera.transform.position = selectedTile.Position + offsetFromTile;


    }


    void ChooseTile(int x, int y)
    { 
        HexTile selectedTile = _grid.GetHexTile(x, y);
        if (selectedTile == null) return;
        
        UnchooseTile(selectedX, selectedY);
        selectedX = x;
        selectedY = y;
        selectedTile.Select();
        
        // Find new camera position
        Vector3 offsetFromTile = new Vector3(0, 10, -10);
        Vector3 position = selectedTile.Position + offsetFromTile;
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
            _grid.SetTile(selectedX, selectedY, selectedTileType);

            gameStateManager.EndTurn();
        }
        else
        {

            // Currently selecting from 0 to 9
            for (int i = 0; i <= 9; i++)
            {
                if (Input.GetKeyDown(i.ToString()) && i < HexTile.TileType.GetValues(typeof(HexTile.TileType)).Length)
                {
                    selectedTileType = (HexTile.TileType)i;
                    Debug.Log("Number Pressed: " + i);  // Logs the number pressed
                    break;
                }
            }
        }
    }
}
