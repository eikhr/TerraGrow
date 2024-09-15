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
            MoveInDirection(Vector3.forward);
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            MoveInDirection(Vector3.back);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            MoveInDirection(Vector3.left);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            MoveInDirection(Vector3.right);
        }
    }

    void MoveInDirection(Vector3 direction)
    {
        Camera mainCamera = Camera.main;
        if (mainCamera == null) return;

        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        cameraForward.y = 0; // Keep movement in the horizontal plane
        cameraRight.y = 0;

        Vector3 moveDirection = direction.z * cameraForward + direction.x * cameraRight;
        moveDirection.Normalize();

        int newX = selectedX + Mathf.RoundToInt(moveDirection.x);
        int newY = selectedY + Mathf.RoundToInt(moveDirection.z);

        ChooseTile(newX, newY);
    }
}
