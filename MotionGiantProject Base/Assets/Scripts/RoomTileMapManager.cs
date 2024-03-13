using System.Collections;
using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using UnityEngine;

public class RoomTileMapManager : MonoBehaviour
{
    [SerializeField] GameObject roomPrefab;
    [SerializeField] GameObject playerPrefab;

    [SerializeField] private int maxRooms = 15;
    [SerializeField] private int minRooms = 10;

    private int gridSizeX = 10;
    private int gridSizeY = 10;

    private Queue<Vector2Int> roomQueue = new Queue<Vector2Int>();
    public Dictionary<string, GameObject> roomDictionary = new Dictionary<string, GameObject>();

    private bool generationComplete = false;

    private Room[,] roomGrid;
    private int roomCount;

    private bool startRoom = false;

    public Room[,] RoomGrid { get => roomGrid; set => roomGrid = value; }

    private void Start()
    {
        RoomGrid = new Room[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();
        createMap();
        createRoomsFromGrid();
    }

    private void createMap()
    {
        //start in the middle of the grid
        //generate a new room
        //try and generate rooms in all adjacent sides
        //repeat for each new room
        //stop when rooms exceed max rooms

        Vector2Int intialRoomIndex = new Vector2Int(gridSizeX / 2, gridSizeY / 2);
        roomQueue.Enqueue(intialRoomIndex);

        RoomGrid[intialRoomIndex.x, intialRoomIndex.y] = new Room($"Room-{roomCount}", intialRoomIndex.x, intialRoomIndex.y, true);
        roomCount++;

        while (!generationComplete)
        {
            if (roomQueue.Count > 0 && roomCount < maxRooms)
            {
                Vector2Int roomIndex = roomQueue.Dequeue();
                int gridX = roomIndex.x;
                int gridY = roomIndex.y;
                TryGenerateRoom(new Vector2Int(gridX - 1, gridY));
                TryGenerateRoom(new Vector2Int(gridX, gridY - 1));
                TryGenerateRoom(new Vector2Int(gridX + 1, gridY));
                TryGenerateRoom(new Vector2Int(gridX, gridY + 1));
            }
            else if (roomCount < minRooms)
            {
                Debug.Log("Rooms were less than min amount");
                RegenerateRooms();
            }
            else if (!generationComplete)
            {
                Debug.Log($"Generation complete, {roomCount} rooms created");
                generationComplete = true;
            }
        }

    }

    private bool TryGenerateRoom(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;

        if (!(0 < x && x < gridSizeX - 1 && 0 < y && y < gridSizeY - 1))
        {
            return false;
        }

        if (roomCount >= maxRooms)
        {
            return false;
        }

        if (Random.value < 0.5f && roomIndex != Vector2Int.zero)
        {
            return false;
        }

        if (CountAdjacentRooms(roomIndex) > 1)
        {
            return false;
        }

        roomQueue.Enqueue(roomIndex);
        RoomGrid[x, y] = new Room($"Room-{roomCount}", x, y, false);
        roomCount++;

        OpenDoors(RoomGrid[x, y], x, y);

        return true;
    }

    private void RegenerateRooms()
    {
        RoomGrid = new Room[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        createMap();
    }

    private void OpenDoors(Room room, int x, int y)
    {
        if (x > 0 && RoomGrid[x - 1, y] != null)
        {
            RoomGrid[x, y].LeftDoor = true;
            RoomGrid[x - 1, y].RightDoor = true;
        }
        if (x < gridSizeX - 1 && RoomGrid[x + 1, y] != null)
        {
            RoomGrid[x, y].RightDoor = true;
            RoomGrid[x + 1, y].LeftDoor = true;
        }
        if (y > 0 && RoomGrid[x, y - 1] != null)
        {
            RoomGrid[x, y].BottomDoor = true;
            RoomGrid[x, y - 1].TopDoor = true;
        }
        if (y < gridSizeY - 1 && RoomGrid[x, y + 1] != null)
        {
            RoomGrid[x, y].TopDoor = true;
            RoomGrid[x, y + 1].BottomDoor = true;
        }
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;
        Debug.Log(x + ", " + y);
        if (0 < x && x < gridSizeX - 1 && 0 < y && y < gridSizeY - 1)
        {
            if (RoomGrid[x - 1, y] != null) count++; // left neighbor
            if (RoomGrid[x + 1, y] != null) count++; // right neighbor
            if (RoomGrid[x, y - 1] != null) count++; // Bottom neighbor
            if (RoomGrid[x, y + 1] != null) count++; // Top neighbor
        }

        return count;
    }

    public void AddRoomToDictionary(GameObject room)
    {
        // Use the room's name as the key
        string roomName = room.name;

        // Check if the roomMap already contains this roomName
        if (!roomDictionary.ContainsKey(roomName))
        {
            // Add the room to the map
            roomDictionary.Add(roomName, room);
        }
        else
        {
            Debug.Log("Room " + roomName + " already exists in the map.");
        }
    }

    private void createRoomsFromGrid()
    {
        for (int c = 0; c < RoomGrid.GetLength(1); c++)
        {
            for (int r = 0; r < RoomGrid.GetLength(0); r++)
            {
            
                if (RoomGrid[r, c] != null)
                {
                    // Add the roomObject prefab to the scene
                    GameObject newRoom = Instantiate(roomPrefab, Vector3.zero, Quaternion.identity);
                    newRoom.name = RoomGrid[r, c].RoomName;
                    AddRoomToDictionary(newRoom);

                }
            }
        }
        for (int c = 0; c < RoomGrid.GetLength(1); c++)
        {
            for (int r = 0; r < RoomGrid.GetLength(0); r++)
            {
                if (RoomGrid[r, c] != null)
                {
                    GameObject roomObject = GameObject.Find(RoomGrid[r, c].RoomName);
                    if (roomObject != null)
                    {
                        RoomObjectScript roomScript = roomObject.GetComponent<RoomObjectScript>();
                        roomScript.currentRoom = roomObject;
                        if (roomScript != null)
                        {
                            roomScript.player = GameObject.FindGameObjectWithTag("Player");
                            if (r > 0 && RoomGrid[r - 1, c] != null)
                            {
                                roomScript.topRoom = roomDictionary[RoomGrid[r - 1, c].RoomName];
                            }
                            if (r < RoomGrid.GetLength(0) - 1 && RoomGrid[r + 1, c] != null)
                            {
                                roomScript.bottomRoom = roomDictionary[RoomGrid[r + 1, c].RoomName];
                            }
                            if (c > 0 && RoomGrid[r, c - 1] != null)
                            {
                                roomScript.leftRoom = roomDictionary[RoomGrid[r, c - 1].RoomName];
                            }
                            if (c < RoomGrid.GetLength(1) - 1 && RoomGrid[r, c + 1] != null)
                            {
                                roomScript.rightRoom = roomDictionary[RoomGrid[r, c + 1].RoomName];
                            }

                            roomScript.room = RoomGrid[r, c];
                            roomScript.OpenDoors();
                            roomScript.assignTransitions();
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<string, GameObject> entry in roomDictionary)
        {
            if (!startRoom)
            {
                entry.Value.SetActive(true);
                startRoom = true;
            }
            else
            {
                entry.Value.SetActive(false);
            }
        }
    }
}
