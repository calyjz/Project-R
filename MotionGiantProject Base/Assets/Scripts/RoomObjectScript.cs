using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomObjectScript : MonoBehaviour
{

    [SerializeField] public GameObject SceneTransitionUp;
    [SerializeField] public GameObject SceneTransitionDown;
    [SerializeField] public GameObject SceneTransitionLeft;
    [SerializeField] public GameObject SceneTransitionRight;
    private Room room;

    public Tilemap tilemap;
    public Tile doorTile;

    public Room RoomGetSet { get => room; set => room = value; }

    public void OpenDoors()
    {
        
        if (room.TopDoor)
        {
            openTopDoor();
        }

        if (room.BottomDoor)
        {
            openBottomDoor();
        }

        if (room.LeftDoor)
        {
            openLeftDoor();
        }

        if (room.RightDoor)
        {
            openRightDoor();
        }
    }

    private void openTopDoor()
    {
        int centerX = tilemap.size.x / 2;
        int centerXP1 = (tilemap.size.x / 2) + 1;
        int centerXM1 = (tilemap.size.x / 2) - 1;
        int topY = tilemap.size.y - 3; // The top row of the tilemap
        Vector3Int doorPosition = new Vector3Int(centerX, topY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(centerXP1, topY, 0);
        Vector3Int doorPositionM1 = new Vector3Int(centerXM1, topY, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionP1, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openBottomDoor()
    {
        int centerX = tilemap.size.x / 2;
        int centerXP1 = (tilemap.size.x / 2) + 1;
        int centerXM1 = (tilemap.size.x / 2) - 1;
        int bottomY = 0; // The bottom row of the tilemap
        Vector3Int doorPosition = new Vector3Int(centerX, bottomY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(centerXP1, bottomY, 0);
        Vector3Int doorPositionM1 = new Vector3Int(centerXM1, bottomY, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionP1, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openLeftDoor()
    {
        int leftX = 0; // The left column of the tilemap
        int centerY = tilemap.size.y / 2;
        int centerYP1 = (tilemap.size.y / 2) + 1;
        int centerYM1 = (tilemap.size.y / 2) - 1;
        Vector3Int doorPosition = new Vector3Int(leftX, centerY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(leftX, centerYP1, 0);
        Vector3Int doorPositionM1 = new Vector3Int(leftX, centerYM1, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }

    private void openRightDoor()
    {
        int rightX = tilemap.size.x - 1; // The right column of the tilemap
        int centerY = tilemap.size.y / 2;
        int centerYP1 = (tilemap.size.y / 2) + 1;
        int centerYM1 = (tilemap.size.y / 2) - 1;
        Vector3Int doorPosition = new Vector3Int(rightX, centerY, 0);
        Vector3Int doorPositionP1 = new Vector3Int(rightX, centerYP1, 0);
        Vector3Int doorPositionM1 = new Vector3Int(rightX, centerYM1, 0);
        tilemap.SetTile(doorPosition, doorTile); // Remove the tile
        tilemap.SetTile(doorPositionM1, doorTile); // Remove the tile
    }
}
