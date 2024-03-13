using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject player;
    public float smoothSpeed = 1f;
    public Vector2 maxPosition;
    public Vector2 minPosition;

    public RoomTileMapManager map;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 desiredPosition = new Vector3(player.transform.position.x, player.transform.position.y, -10);
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minPosition.x, maxPosition.x);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minPosition.y, maxPosition.y);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothedPosition;

        Renderer renderer = player.GetComponent<Renderer>();
        Vector3 size = renderer.bounds.size;

        Vector2 playerPosition = player.transform.position;
        //if ((playerPosition.x - 9.78) - size.x / 2 > maxPosition.x)
        //{
        //    maxPosition.x += map.getRoomWidth();
        //    minPosition.x += map.getRoomWidth();
        //}

        //if (playerPosition.x + 9.78 < minPosition.x)
        //{
        //    maxPosition.x -= map.getRoomWidth();
        //    minPosition.x -= map.getRoomWidth();
        //}

        //if (playerPosition.y - 6.46 > maxPosition.y)
        //{
        //    maxPosition.y += map.getRoomHeight();
        //    minPosition.y += map.getRoomHeight();
        //}

        //if (playerPosition.y + 6.46 < minPosition.y)
        //{
        //    maxPosition.y -= map.getRoomHeight();
        //    minPosition.y -= map.getRoomHeight();
        //}
    }
}
