using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed = 10f;
    public float scrollSpeed = 1000f;
    public float rotationSpeed = 5f;

    private Camera targetCamera;

    void Start()
    {
        // Dynamically find the main camera
        targetCamera = Camera.main;
    }

    void Update()
    {
        if (targetCamera == null)
        {
            Debug.LogError("No camera found.");
            return;
        }

        // Movement with W, A, S, D or arrow keys
        float horizontal = Input.GetAxis("Horizontal");  // A, D or Left/Right Arrow
        float vertical = Input.GetAxis("Vertical");  // W, S or Up/Down Arrow

        Vector3 move = new Vector3(horizontal, 0, vertical);
        targetCamera.transform.Translate(move * moveSpeed * Time.deltaTime, Space.Self);

        // Zoom with Mouse Scroll Wheel
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        targetCamera.transform.Translate(Vector3.forward * scroll * scrollSpeed * Time.deltaTime, Space.Self);

        // Right-click drag for rotation
        if (Input.GetMouseButton(1))
        {
            float rotationX = Input.GetAxis("Mouse X") * rotationSpeed;
            float rotationY = Input.GetAxis("Mouse Y") * rotationSpeed;

            targetCamera.transform.Rotate(Vector3.up, rotationX, Space.World);
            targetCamera.transform.Rotate(Vector3.right, -rotationY, Space.Self);
        }
    }
}
