using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public int rayCount = 12; // Number of rays to cast
    public float rayLength = 10f; // Length of the rays
    public LayerMask layerMask; // Layer mask to filter what objects the rays can hit
    private List<Vector2> hitPoints = new List<Vector2>();

    //private SpriteRenderer spriteRenderer;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        hitPoints.Clear();

        // Calculate the angle between each ray
        float angleStep = 360f / rayCount;

        // Cast rays from the center in a circle
        for (int i = 0; i < rayCount; i++)
        {
            // Calculate the direction of the current ray
            Vector3 direction = Quaternion.Euler(0f, 0f, i * angleStep) * Vector3.up;

            // Perform raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, rayLength, layerMask);

            // If the ray hits something, add the hit point to the list
            if (hit.collider != null)
            {
                hitPoints.Add(hit.point - (Vector2)transform.position); // Store hit point relative to the center
            }
            else
            {
                // If the ray doesn't hit anything, add a point at the ray's maximum length
                hitPoints.Add(direction.normalized * rayLength);
            }
        }

        // Update the polygon sprite
        UpdatePolygonSprite();
    }
    void UpdatePolygonSprite()
    {
        // Create a new PolygonCollider2D and add points to it
        //PolygonCollider2D polygonCollider = gameObject.AddComponent<PolygonCollider2D>();
        //polygonCollider.points = hitPoints.ToArray();

        //// Generate a sprite from the collider's shape
        //Texture2D texture = new Texture2D(1, 1);
        //texture.SetPixel(0, 0, Color.white);
        //texture.Apply();
        //spriteRenderer.sprite = Sprite.Create(texture, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);

        //// Destroy the collider to avoid memory leak
        //Destroy(polygonCollider);
        for (int i = 0; i < hitPoints.Count; i++)
        {
            Debug.Log(hitPoints[i]);
        }
    }
}
