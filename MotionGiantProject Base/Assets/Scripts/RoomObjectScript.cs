using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class RoomObjectScript : MonoBehaviour
{
    public Room room;
    public GameObject player;

    public Tilemap tilemap;
    public Tile doorTile;

    public GameObject currentRoom;
    public GameObject leftRoom;
    public GameObject rightRoom;
    public GameObject topRoom;
    public GameObject bottomRoom;

    [Header("Room Transitions")]
    [SerializeField] public GameObject RoomTransitionUp;
    [SerializeField] public GameObject RoomTransitionDown;
    [SerializeField] public GameObject RoomTransitionLeft;
    [SerializeField] public GameObject RoomTransitionRight;

    [Header("Room SpawnPoints")]
    [SerializeField] public GameObject SpawnPointUp;
    [SerializeField] public GameObject SpawnPointDown;
    [SerializeField] public GameObject SpawnPointLeft;
    [SerializeField] public GameObject SpawnPointRight;

    public void assignTransitions()
    {
        if (RoomTransitionUp != null)
        {
            RoomTransitionInScene upScript = RoomTransitionUp.GetComponent<RoomTransitionInScene>();
            if (upScript != null)
            {
                upScript.player = player;
                upScript.currentRoom = currentRoom;
                if (topRoom != null)
                {
                    upScript.nextRoom = topRoom;
                    RoomObjectScript topRoomScript = topRoom.GetComponent<RoomObjectScript>();
                    if (topRoomScript != null && topRoomScript.SpawnPointDown != null)
                    {
                        upScript.nextRoomSpawnPoint = topRoomScript.SpawnPointDown.transform.position;
                    }
                }
            }
        }

        if (RoomTransitionDown != null)
        {
            RoomTransitionInScene downScript = RoomTransitionDown.GetComponent<RoomTransitionInScene>();
            if (downScript != null)
            {
                downScript.player = player;
                downScript.currentRoom = currentRoom;
                if (bottomRoom != null)
                {
                    downScript.nextRoom = bottomRoom;
                    RoomObjectScript bottomRoomScript = bottomRoom.GetComponent<RoomObjectScript>();
                    if (bottomRoomScript != null && bottomRoomScript.SpawnPointUp != null)
                    {
                        downScript.nextRoomSpawnPoint = bottomRoomScript.SpawnPointUp.transform.position;
                    }
                }
            }
        }

        if (RoomTransitionLeft != null)
        {
            RoomTransitionInScene leftScript = RoomTransitionLeft.GetComponent<RoomTransitionInScene>();
            if (leftScript != null)
            {
                leftScript.player = player;
                leftScript.currentRoom = currentRoom;
                if (leftRoom != null)
                {
                    leftScript.nextRoom = leftRoom;
                    RoomObjectScript leftRoomScript = leftRoom.GetComponent<RoomObjectScript>();
                    if (leftRoomScript != null && leftRoomScript.SpawnPointRight != null)
                    {
                        leftScript.nextRoomSpawnPoint = leftRoomScript.SpawnPointLeft.transform.position;
                    }
                }
            }
        }

        if (RoomTransitionRight != null)
        {
            RoomTransitionInScene rightScript = RoomTransitionRight.GetComponent<RoomTransitionInScene>();
            if (rightScript != null)
            {
                rightScript.player = player;
                rightScript.currentRoom = currentRoom;
                if (rightRoom != null)
                {
                    rightScript.nextRoom = rightRoom;
                    RoomObjectScript rightRoomScript = rightRoom.GetComponent<RoomObjectScript>();
                    if (rightRoomScript != null && rightRoomScript.SpawnPointLeft != null)
                    {
                        rightScript.nextRoomSpawnPoint = rightRoomScript.SpawnPointRight.transform.position;
                    }
                }
            }
        }
    }

    public void OpenDoors()
    {
        //Order is wrong for some reason, that is why the things don't line up, it all works though
        if (room.TopDoor)
        {
            
            openLeftDoor();
        }

        if (room.BottomDoor)
        {

            openRightDoor();
        }

        if (room.LeftDoor)
        {
            openTopDoor();
        }

        if (room.RightDoor)
        {
            openBottomDoor();
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
