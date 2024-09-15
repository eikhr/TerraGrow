using UnityEngine;
using UnityEngine.UI;

public class Itembar : MonoBehaviour
{
    public GameObject[] prefabs;        // The 3D Prefabs to display
    public int[] prefabNumbers;         // Number of each prefab to display
    private RenderTexture[] renderTextures; // Render textures for each prefab
    private Image[] itemImages;         // UI Images in the menu bar
    private Text[] itemCounts;          // Text components for item counts
    private Camera renderCamera;        // Camera to render prefab thumbnails
    private Canvas canvas;              // The dynamically created Canvas
    private int selectedItemIndex = -1;
    private Image[] borderImages;       // Border images for highlighting selected item

    public GameStateManager gameStateManager;

    // Score
    private RectTransform panelTransform; // The UI panel where the score will be shown
    private Text scoreText;               // Text component to display the score
    private int score = 0;                // Variable to track the score

    void Start()
    {
        // Create the Render Camera for prefab thumbnails
        CreateRenderCamera();

        // Create the Canvas and UI for the menu bar
        CreateCanvasAndMenu();

        // Create RenderTextures for each prefab and display them
        CreateRenderTextures();

        // Create the score text
        CreateScoreText();

        UpdateScore();

        UpdateItemCounts();
    }

    void Update()
    {
        HandleInput();
        HighlightSelectedItem();
        UpdateScore();
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
        renderCamera.transform.position = new Vector3(0, 500, -10); // Position the camera
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
        panelTransform = panel.AddComponent<RectTransform>(); // Assign to class member
        panelTransform.SetParent(canvas.transform);

        // Adjust panel anchors and pivot to center
        panelTransform.anchorMin = new Vector2(0.5f, 0);
        panelTransform.anchorMax = new Vector2(0.5f, 0);
        panelTransform.pivot = new Vector2(0.5f, 0.5f);
        panelTransform.anchoredPosition = new Vector2(0, 50);

        // Set the panel's width based on the total width of items and spacing
        float imageSize = 100; // Icon size
        float spacing = -40f;    // Reduced spacing between icons
        int itemCount = prefabs.Length;
        float totalWidth = (imageSize * itemCount) + (spacing * (itemCount - 1));
        panelTransform.sizeDelta = new Vector2(totalWidth, imageSize);

        // Create Image UI elements dynamically
        itemImages = new Image[itemCount];
        borderImages = new Image[itemCount];
        itemCounts = new Text[itemCount];
        for (int i = 0; i < itemCount; i++)
        {
            // Create parent GameObject for border
            GameObject borderObj = new GameObject("ItemBorder" + i);
            RectTransform borderTransform = borderObj.AddComponent<RectTransform>();
            borderTransform.SetParent(panelTransform);

            // Set the size of the border (slightly larger than the image)
            float borderSize = imageSize - 50f; // Adjust as needed for border thickness
            borderTransform.sizeDelta = new Vector2(borderSize, borderSize - 20f);

            // Calculate and set the position
            float xPosition = (-totalWidth / 2) + (imageSize / 2) + i * (imageSize + spacing);
            borderTransform.anchoredPosition = new Vector2(xPosition, 0);

            // Set anchor and pivot to center
            borderTransform.anchorMin = new Vector2(0.5f, 0.5f);
            borderTransform.anchorMax = new Vector2(0.5f, 0.5f);
            borderTransform.pivot = new Vector2(0.5f, 0.5f);

            // Add Image component to border and set color to transparent initially
            Image borderImage = borderObj.AddComponent<Image>();
            borderImage.sprite = Resources.GetBuiltinResource<Sprite>("UI/Skin/UISprite.psd");
            borderImage.type = Image.Type.Sliced;
            borderImage.color = Color.clear; // Transparent by default
            borderImages[i] = borderImage;

            // Create child GameObject for the item image
            GameObject imageObj = new GameObject("ItemImage" + i);
            itemImages[i] = imageObj.AddComponent<Image>();
            RectTransform imgTransform = imageObj.GetComponent<RectTransform>();
            imgTransform.SetParent(borderTransform);

            // Set the size of the image (slightly smaller than border)
            imgTransform.sizeDelta = new Vector2(imageSize, imageSize);
            imgTransform.anchoredPosition = Vector2.zero; // Centered within parent

            // Set anchor and pivot to center
            imgTransform.anchorMin = new Vector2(0.5f, 0.5f);
            imgTransform.anchorMax = new Vector2(0.5f, 0.5f);
            imgTransform.pivot = new Vector2(0.5f, 0.5f);

            // Create child GameObject for the item count text
            GameObject textObj = new GameObject("ItemCount" + i);
            itemCounts[i] = textObj.AddComponent<Text>();
            RectTransform textTransform = textObj.GetComponent<RectTransform>();
            textTransform.SetParent(borderTransform);

            // Set the size and position of the text
            textTransform.sizeDelta = new Vector2(imageSize, 20);
            textTransform.anchoredPosition = new Vector2(0, -imageSize / 2 - 10); // Below the image

            // Set anchor and pivot to center
            textTransform.anchorMin = new Vector2(0.5f, 0.5f);
            textTransform.anchorMax = new Vector2(0.5f, 0.5f);
            textTransform.pivot = new Vector2(0.5f, 0.5f);
            textTransform.localPosition = new Vector3(0, -20, 0); // Relative to the camera

            // Configure the text component
            itemCounts[i].font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
            itemCounts[i].fontSize = 14;
            itemCounts[i].color = Color.white;
            itemCounts[i].alignment = TextAnchor.MiddleCenter;
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
            // Adjust the position to center the prefab in the thumbnail
            float yPositionOffset = -0.3f; // Adjust this value as needed
            Vector3 prefabPosition = renderCamera.transform.position + renderCamera.transform.forward * 5 + Vector3.up * yPositionOffset;

            GameObject prefabInstance = Instantiate(prefabs[i], prefabPosition, Quaternion.Euler(-22, 0, 0));
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlaceTile();
        }
    }

    void PlaceTile()
    {
        if (selectedItemIndex < 0 || selectedItemIndex >= prefabs.Length)
        {
            Debug.Log("No item selected!");
            return;
        }

        if (prefabNumbers[selectedItemIndex] <= 0)
        {
            Debug.Log("No more of this item left!");
            return;
        }

        prefabNumbers[selectedItemIndex]--;
        gameStateManager.PlaceTile(prefabs[selectedItemIndex]);
        UpdateItemCounts();
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
            if (i == selectedItemIndex)
            {
                // Set border color to white
                borderImages[i].color = Color.white;
            }
            else
            {
                // Set border color to transparent
                borderImages[i].color = Color.clear;
            }
        }
    }

    void UpdateItemCounts()
    {
        for (int i = 0; i < itemCounts.Length; i++)
        {
            itemCounts[i].text = prefabNumbers[i].ToString();
        }
    }

    void CreateScoreText()
    {
        // Create a new GameObject for the score text
        GameObject scoreObj = new GameObject("ScoreText");

        // Add RectTransform component to position the text in the UI
        RectTransform scoreTransform = scoreObj.AddComponent<RectTransform>();

        // Set the parent to the canvas
        scoreTransform.SetParent(canvas.transform);

        // Set the size and anchored position of the score text
        scoreTransform.sizeDelta = new Vector2(400, 100);  // Adjust as necessary
        scoreTransform.anchoredPosition = new Vector2(0, -150);  // Position below the menu bar

        // Set anchor and pivot to center
        scoreTransform.anchorMin = new Vector2(0.5f, 0.5f);
        scoreTransform.anchorMax = new Vector2(0.5f, 0.5f);
        scoreTransform.pivot = new Vector2(0.5f, 0.5f);

        // Add a Text component to display the score
        scoreText = scoreObj.AddComponent<Text>();

        // Set up the Text component properties
        scoreText.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");  // Default Unity font
        scoreText.fontSize = 50;  // Adjust the font size as needed
        scoreText.color = Color.white;  // Set the text color
        scoreText.alignment = TextAnchor.MiddleCenter;  // Centered text
        scoreText.text = "Score: " + score.ToString();
    }

    void UpdateScore()
    {
        // Update the score text display
        scoreText.text = "Score: " + score.ToString();
    }

    public void AddScore(int amount)
    {
        score += amount;
        UpdateScore();
    }

    void SetScore(int newScore)
    {
        score = newScore;
        UpdateScore();
    }
}
