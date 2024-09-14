using UnityEngine;
using UnityEngine.UI;

public class Itembar : MonoBehaviour
{
    public GameObject[] prefabs;  // The 3D Prefabs to display
    private RenderTexture[] renderTextures; // Render textures for each prefab
    private Image[] itemImages;  // UI Images in the menu bar
    private Camera renderCamera; // Camera to render prefab thumbnails
    private Canvas canvas; // The dynamically created Canvas
    private int selectedItemIndex = -1;

    void Start()
    {
        // Create the Render Camera for prefab thumbnails
        CreateRenderCamera();

        // Create the Canvas and UI for the menu bar
        CreateCanvasAndMenu();

        // Create RenderTextures for each prefab and display them
        CreateRenderTextures();
    }

    void Update()
    {
        HandleInput();
        HighlightSelectedItem();
    }

    // Create a render camera for generating prefab thumbnails
    void CreateRenderCamera()
    {
        GameObject camObj = new GameObject("RenderCamera");
        renderCamera = camObj.AddComponent<Camera>();
        renderCamera.clearFlags = CameraClearFlags.SolidColor;
        renderCamera.backgroundColor = Color.clear;
        renderCamera.orthographic = true;
        renderCamera.enabled = false; // Disable as we only use it to render thumbnails
    }

    // Create the Canvas and Image UI elements programmatically
    void CreateCanvasAndMenu()
    {
        // Create Canvas
        GameObject canvasObj = new GameObject("MenuCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObj.AddComponent<CanvasScaler>().uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasObj.AddComponent<GraphicRaycaster>();
    
        // Create the panel (menu bar)
        GameObject panel = new GameObject("MenuPanel");
        RectTransform panelTransform = panel.AddComponent<RectTransform>();
        panelTransform.SetParent(canvas.transform);
    
        // Adjust panel anchors and pivot to center
        panelTransform.anchorMin = new Vector2(0.5f, 0);
        panelTransform.anchorMax = new Vector2(0.5f, 0);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panelTransform.anchoredPosition = new Vector2(0, 50);
    
        // Set the panel's width based on the total width of items and spacing
        float imageSize = 100; // Icon size
        float spacing = 10;    // Spacing between icons
        int itemCount = prefabs.Length;
        float totalWidth = (imageSize * itemCount) + (spacing * (itemCount - 1));
        panelTransform.sizeDelta = new Vector2(totalWidth, imageSize);
    
        // Create Image UI elements dynamically
        itemImages = new Image[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            GameObject imageObj = new GameObject("ItemImage" + i);
            itemImages[i] = imageObj.AddComponent<Image>();
            RectTransform imgTransform = imageObj.GetComponent<RectTransform>();
            imgTransform.SetParent(panelTransform);
    
            // Set the size of the image
            imgTransform.sizeDelta = new Vector2(imageSize, imageSize);
    
            // Calculate and set the position
            float xPosition = (-totalWidth / 2) + (imageSize / 2) + i * (imageSize + spacing);
            imgTransform.anchoredPosition = new Vector2(xPosition, 0);
        }
    }








    // Create RenderTextures for each prefab and assign them to the UI
    void CreateRenderTextures()
    {
        renderTextures = new RenderTexture[prefabs.Length];
    
        for (int i = 0; i < prefabs.Length; i++)
        {
            // Adjust RenderTexture size to match icon size for better quality
            int textureSize = 256; // Or match your icon size if needed
            renderTextures[i] = new RenderTexture(textureSize, textureSize, 16);
            renderCamera.targetTexture = renderTextures[i];
    
            // Instantiate the prefab and position it
            GameObject prefabInstance = Instantiate(prefabs[i], renderCamera.transform.position + renderCamera.transform.forward * 5, Quaternion.Euler(-22, 0, 0));
            prefabInstance.transform.localScale = Vector3.one * 3.5f; // Adjust as needed
    
            // Render the prefab into the texture
            renderCamera.Render();
    
            // Destroy the prefab immediately after rendering
            DestroyImmediate(prefabInstance);
    
            // Create a Sprite from the RenderTexture and assign it to the UI Image
            Texture2D texture = new Texture2D(renderTextures[i].width, renderTextures[i].height, TextureFormat.RGBA32, false);
            RenderTexture.active = renderTextures[i];
            texture.ReadPixels(new Rect(0, 0, renderTextures[i].width, renderTextures[i].height), 0, 0);
            texture.Apply();
            RenderTexture.active = null;
    
            Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            itemImages[i].sprite = sprite;
        }
    }



    void HandleInput()
    {
        // Map keys Alpha1 to Alpha9 (1-9) and Alpha0 (0) to select items
        int maxKeys = Mathf.Min(prefabs.Length, 10); // Limit to 10 items
        for (int i = 0; i < maxKeys; i++)
        {
            KeyCode keyCode;
            if (i == 9) // Index 9 corresponds to the 10th item, which is key '0'
                keyCode = KeyCode.Alpha0;
            else
                keyCode = KeyCode.Alpha1 + i; // KeyCode.Alpha1 + 0 = Alpha1, etc.
    
            if (Input.GetKeyDown(keyCode))
            {
                SelectItem(i);
            }
        }
    }


    void SelectItem(int index)
    {
        if (index >= 0 && index < prefabs.Length)
        {
            selectedItemIndex = index;
            Debug.Log("Selected Item: " + prefabs[index].name);
            // You can instantiate or perform further actions with the selected prefab here
        }
    }

    void HighlightSelectedItem()
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            itemImages[i].color = (i == selectedItemIndex) ? Color.yellow : Color.white;
        }
    }
}
