using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //singleton pattern
    private static GameController _instance;
    public static GameController Instance { get { return _instance; } }

    //creates a public enum that contains the current game state
    public GameState state;
    //{N, E, S, W}, index refers to the room# -1
    public int[,] roomDoors = {{0, 0, 0, 2}, { 3, 1, 0, 0 }, { 0, 0, 2, 0 } };

    public int currentRoom = 0;

    GameObject Player;
    GameObject MainCamera;

    private void Awake()
    {
        _instance = this;
        UpdateGameState(GameState.Start);
        Player = GameObject.FindGameObjectWithTag("Player");
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        if (Player == null)
        {
            Debug.Log("Player object not found");
        }
        Player.SetActive(false);

    }

    public void UpdateGameState(GameState newState)
    {//Updating the current game state
        state = newState;

        switch (newState)
        {
            case GameState.Start:
                //Call function to load the starting screen
                loadStartingScreen();
                break;
            case GameState.Run:
                //Call function to start the run
                StartRun();
                break;
            case GameState.Respawn:
                //call function to reset the layout and restart the run
                break;
            case GameState.Win:
                //call function when player wins
                break;
            default:
                Debug.Log("game state" + newState.ToString() + "not recognized");
                break;
        }

    }

    public void loadStartingScreen()
    {//Loads the starting screen
        DontDestroyOnLoad(this);
    }

    public void StartRun()
    {//called at the start of each run
        //TODO: Create room layout here

        currentRoom = 1;

        //Makes player object visible and loads first scene
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(MainCamera);
        SceneManager.LoadScene("Room1");
        Player.SetActive(true);


    }

    public void loadNextRoom(int DoorDirection)
    {//loads the next room
        int nextRoom = roomDoors[currentRoom-1, DoorDirection];

        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
        {
            Debug.Log("Cannot find player object");
        }
        
        //debug message
        Debug.Log("Exiting Room" + currentRoom.ToString() + "through D" + DoorDirection.ToString() + ", entering Room" + nextRoom.ToString());

        //loads new scene
        DontDestroyOnLoad(Player);
        SceneManager.LoadScene("Room" + nextRoom.ToString());
        currentRoom = nextRoom;

        //set new player position
        switch (DoorDirection)
        {
            case 0://set player in front of South door
                Player.transform.position = new Vector3(0, (float)-3.6, (float)-1.1);
                break;
            case 1://set player in front of West door
                Player.transform.position = new Vector3((float)8.89, 0, (float)-1.1);
                break;
            case 2://set player in front of North door
                Player.transform.position = new Vector3(0, (float)3.6, (float)-1.1);
                break;
            case 3://set player in front of East door
                Player.transform.position = new Vector3((float)-8.89, 0, (float)-1.1);
                break;
            default:
                Debug.Log("incorrect door direction variable");
                break;

        }



    }

    public int GetCurrentRoom()
    {//helper function to retrieve the current room #
        return currentRoom;
    }

    void PlayerRespawn()
    {
        Player.SetActive(false);
        SceneManager.LoadScene("Respawn");
        
    }
}

public enum GameState
{
    Start,
    Run,
    Respawn,
    Win
}
