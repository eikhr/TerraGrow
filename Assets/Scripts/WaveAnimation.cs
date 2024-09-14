using UnityEngine;

public class WaveAnimation : MonoBehaviour
{
    public float waveSpeed = 1.0f;    // Speed of the waves
    public float waveHeight = 0.5f;   // Height of the waves
    public float waveFrequency = 1.0f; // Frequency of the waves

    private Mesh mesh;
    private Vector3[] originalVertices;

    void Start()
    {
        // Get the mesh from the plane
        mesh = GetComponent<MeshFilter>().mesh;

        // Store the original vertices
        originalVertices = mesh.vertices;
    }

    void Update()
    {
        AnimateWaves();
    }

    void AnimateWaves()
    {
        // Create a copy of the original vertices
        Vector3[] displacedVertices = new Vector3[originalVertices.Length];

        // Loop through each vertex and apply a sine wave to create the wave effect
        for (int i = 0; i < originalVertices.Length; i++)
        {
            Vector3 vertex = originalVertices[i];

            // Create wave motion based on the X and Z coordinates of the vertex
            vertex.y = Mathf.Sin(Time.time * waveSpeed + vertex.x * waveFrequency) * waveHeight;

            // Assign the displaced vertex back to the array
            displacedVertices[i] = vertex;
        }

        // Update the mesh with the new vertices
        mesh.vertices = displacedVertices;

        // Recalculate normals to ensure lighting works properly
        mesh.RecalculateNormals();

        // Apply the updated mesh back to the mesh filter
        GetComponent<MeshFilter>().mesh = mesh;
    }
}
