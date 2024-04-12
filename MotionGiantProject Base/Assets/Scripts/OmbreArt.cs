using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class OmbreArt : MonoBehaviour
{
    public int textureSize = 512; // Size of the texture
    public ColorStop[] colorStops; // Array of color stops
    public Color firstColor;

    public float ombreScale = 1f;
    [System.Serializable]
    public struct ColorStop
    {
        [Range(0f, 1f)]
        public float position; // Position of the color stop as a percentage of the radius
        public Color color; // Color of the stop
    }
    private bool loaded = false;
    // Start is called before the first frame update
    void DrawTexture()
    {
        Texture2D texture = new Texture2D(textureSize, textureSize);

        // Calculate center position
        Vector2 center = new Vector2(textureSize / 2, textureSize / 2);

        // Loop through each pixel in the texture
        for (int y = 0; y < textureSize; y++)
        {
            for (int x = 0; x < textureSize; x++)
            {
                // Calculate the distance from the current pixel to the center
                float distance = Vector2.Distance(new Vector2(x, y), center) / (textureSize / 2);

                // Determine color based on color stops
                Color color = firstColor; // Default color
                for (int i = 1; i < colorStops.Length; i++)
                {
                    if (distance <= colorStops[i].position)
                    {
                        float scaledPosition = Mathf.Clamp01((distance - colorStops[i - 1].position) / ombreScale + colorStops[i - 1].position);
                        float t = Mathf.InverseLerp(colorStops[i - 1].position, colorStops[i].position, scaledPosition);
                        color = Color.Lerp(colorStops[i - 1].color, colorStops[i].color, t);
                        break;
                    }
                }

                // Set the color of the current pixel in the texture
                texture.SetPixel(x, y, color);
            }
        }

        // Apply changes and assign the texture to a material
        texture.Apply();
        //GetComponent<Renderer>().material.mainTexture = texture;
        GetComponent<Renderer>().sharedMaterial.mainTexture = texture;
    }
    void Awake()
    {
        DrawTexture();
    }
    public void ChangeColor(Color color2, int index)
    {
        colorStops[index].color = color2;
        DrawTexture();
    }
    public void ChangeAlpha(float a)
    {
        for (int i = 0; i < colorStops.Length; i += 1)
        {
            colorStops[i].color.a = a;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!loaded)
        {
            DrawTexture();
            loaded = true;
        }
        
        // Create a new texture
        //DrawTexture();
        // Useful in Edit Mode
        //DrawTexture();
        //GetComponent<Renderer>().material.mainTexture = texture;
    }
}
