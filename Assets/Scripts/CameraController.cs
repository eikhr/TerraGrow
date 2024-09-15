using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float scrollSpeed = 1000f;
    public float rotationSpeed = 5f;

    private Camera _targetCamera;
    private Vector3 _goalPosition = Vector3.zero;

    void Start()
    {
        // Dynamically find the main camera
        _targetCamera = Camera.main;
    }

    void Update()
    {
        if (_targetCamera == null)
        {
            Debug.LogError("No camera found.");
            return;
        }
        
        // Move towards the goal position
        if (_goalPosition != Vector3.zero)
        {
            var distance = Vector3.Distance(_targetCamera.transform.position, _goalPosition);
            _targetCamera.transform.position = Vector3.MoveTowards(_targetCamera.transform.position, _goalPosition,
                moveSpeed * Time.deltaTime * distance);
        }
        // Movement with arrow keys
        float horizontal = Input.GetKey(KeyCode.LeftArrow) ? -1 : Input.GetKey(KeyCode.RightArrow) ? 1 : 0;
        float vertical = Input.GetKey(KeyCode.UpArrow) ? 1 : Input.GetKey(KeyCode.DownArrow) ? -1 : 0;

        Vector3 move = new Vector3(horizontal, 0, vertical);
        _targetCamera.transform.Translate(move * moveSpeed * Time.deltaTime, Space.Self);

        // Zoom with Mouse Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        _targetCamera.transform.Translate(Vector3.forward * scroll * scrollSpeed * Time.deltaTime, Space.Self);

        // Right-click drag for rotation
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            _targetCamera.transform.Rotate(Vector3.up, rotationX, Space.World);
            _targetCamera.transform.Rotate(Vector3.right, -rotationY, Space.Self);
        }
    }
    
    public void MoveTowards(Vector3 position)
    {
        _goalPosition = position;
    }

    public void SetCameraPosition(Vector3 position)
    {
        _targetCamera.transform.position = position;
    }

    public void LookAt(Vector3 position)
    {
        _targetCamera.transform.LookAt(position);
    }

    public void DebugSomeShit() {
        Debug.Log("SOmeshiet");
    }
}
