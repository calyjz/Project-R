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
    private List<GameObject> roomObjects = new List<GameObject>();
    private bool generationComplete = false;

    private Room[,] roomGrid;
    private int roomCount;

    private bool startRoom = false;

    private void Start()
    {
        roomGrid = new Room[gridSizeX, gridSizeY];
        roomQueue = new Queue<Vector2Int>();
        createMap();
        createScenesFromGrid();

        //SceneManager.LoadSceneAsync(roomGrid[gridSizeX / 2, gridSizeY / 2].RoomScene.name);
        //for (int r = 0; r < roomGrid.GetLength(0); r++)
        //{
        //    for (int c = 0; c < roomGrid.GetLength(1); c++)
        //    {
        //        if (roomGrid[r, c] != null)
        //        {
        //            Debug.Log(r + ", " + c + ", " + roomGrid[r, c].RoomName + ", " +
        //                      roomGrid[r, c].TopDoor + ", " + roomGrid[r, c].BottomDoor + ", " +
        //                      roomGrid[r, c].LeftDoor + ", " + roomGrid[r, c].RightDoor);
        //        }
        //    }
        //}
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

        roomGrid[intialRoomIndex.x, intialRoomIndex.y] = new Room($"Room-{roomCount}", intialRoomIndex.x, intialRoomIndex.y, true);
        roomCount++;

        while(!generationComplete)
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
        roomGrid[x, y] = new Room($"Room-{roomCount}", x, y, false);
        roomCount++;

        OpenDoors(roomGrid[x, y], x, y);

        return true;
    }

    private void RegenerateRooms()
    {
        roomGrid = new Room[gridSizeX, gridSizeY];
        roomQueue.Clear();
        roomCount = 0;
        generationComplete = false;

        createMap();
    }

    private void OpenDoors(Room room, int x, int y)
    {
        if (x > 0 && roomGrid[x - 1, y] != null)
        {
            roomGrid[x, y].LeftDoor = true;
            roomGrid[x - 1, y].RightDoor = true;
        }
        if (x < gridSizeX - 1 && roomGrid[x + 1, y] != null)
        {
            roomGrid[x, y].RightDoor = true;
            roomGrid[x + 1, y].LeftDoor = true;
        }
        if (y > 0 && roomGrid[x, y - 1] != null)
        {
            roomGrid[x, y].BottomDoor = true;
            roomGrid[x, y - 1].TopDoor = true;
        }
        if (y < gridSizeY - 1 && roomGrid[x, y + 1] != null)
        {
            roomGrid[x, y].TopDoor = true;
            roomGrid[x, y + 1].BottomDoor = true;
        }
    }

    private int CountAdjacentRooms(Vector2Int roomIndex)
    {
        int x = roomIndex.x;
        int y = roomIndex.y;
        int count = 0;
        Debug.Log(x + ", " + y);
        if (0 < x && x < gridSizeX - 1 && 0 < y && y < gridSizeY - 1) {
            if (roomGrid[x - 1, y] != null) count++; // left neighbor
            if (roomGrid[x + 1, y] != null) count++; // right neighbor
            if (roomGrid[x, y - 1] != null) count++; // Bottom neighbor
            if (roomGrid[x, y + 1] != null) count++; // Top neighbor
        }

        return count;
    }

    private void createScenesFromGrid()
    {
        for (int r = 0; r < roomGrid.GetLength(0); r++)
        {
            for (int c = 0; c < roomGrid.GetLength(1); c++)
            {
                if (roomGrid[r, c] != null)
                {
                    // Add the roomObject prefab to the scene
                    GameObject newRoom = Instantiate(roomPrefab);
                    newRoom.name = roomGrid[r, c].RoomName;
                    RoomObjectScript newRoomScript = newRoom.GetComponent<RoomObjectScript>();

                    newRoomScript.RoomGetSet = roomGrid[r, c];

                    if (!startRoom)
                    {
                        newRoom.SetActive(true);
                        startRoom = true;
                    } else
                    {
                        newRoom.SetActive(false);
                    }
                }
            }
        }
        for (int r = 0; r < roomGrid.GetLength(0); r++)
        {
            for (int c = 0; c < roomGrid.GetLength(1); c++)
            {
                if (roomGrid[r, c] != null)
                {
                    GameObject roomObject = GameObject.Find(roomGrid[r, c].RoomName);

                    if (roomObject != null)
                    {
                        RoomObjectScript roomScript = roomObject.GetComponent<RoomObjectScript>();

                        roomScript.OpenDoors();

                    }

                }
            }
        }
    }
}

