using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Room
{
    private string roomName;
    private Scene roomScene;

    private int gridXPosition;
    private int gridYPosition;

    private bool topDoor;
    private bool bottomDoor;
    private bool leftDoor;
    private bool rightDoor;

    private bool startRoom;
    private bool endRoom;

    public Room(string roomName, int gridXPosition, int gridYPosition, bool startRoom)
    {
        this.startRoom = startRoom;
        this.roomName = roomName;
        this.gridXPosition = gridXPosition;
        this.gridYPosition = gridYPosition;
    }

    public int GridXPosition { get => gridXPosition; set => gridXPosition = value; }
    public int GridYPosition { get => gridYPosition; set => gridYPosition = value; }
    public bool TopDoor { get => topDoor; set => topDoor = value; }
    public bool BottomDoor { get => bottomDoor; set => bottomDoor = value; }
    public bool LeftDoor { get => leftDoor; set => leftDoor = value; }
    public bool RightDoor { get => rightDoor; set => rightDoor = value; }
    public string RoomName { get => roomName; set => roomName = value; }
    public bool StartRoom { get => startRoom; set => startRoom = value; }
    public bool EndRoom { get => endRoom; set => endRoom = value; }
    public Scene RoomScene { get => roomScene; set => roomScene = value; }
}
