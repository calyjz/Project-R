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

    // The player statistics that last for the duration of the game
    public static bool death;
    public static bool canTakeDamage = true;
    public static int exp = 50; // Default XP
    public static int hp_max = 100;
    public static float dashCooldown = 0.6f;
    public static float attackPower = 1.00f;
    public static float lightDecrease = 0.5f;

    //variable storing the total number of enemies in a game
    public int numOfEnemies = 4;
    //variable storing the total number of lanterns in a game
    public int numOfLanterns = 8;

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
                death = false;
                StartRun();
                Player.GetComponent<Player>().resetMe();
                break;
            case GameState.Respawn:
                //call function to reset the layout and restart the run
                death = true;
                PlayerRespawn();
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

        //sets all enemy states to undefeated (1 for defeated, 0 for undefeated)
        for (int i = 0; i < numOfEnemies; i++)
        {
            //Debug.Log("creating " + "Enemy_no key " + (i + 1).ToString());
            PlayerPrefs.SetInt("Enemy_no " + (i + 1).ToString(), 0);
        }

        //displays all lanterns (1 for removed, 0 for displayed)
        for (int i = 0; i < numOfLanterns; i++)
        {
            //Debug.Log("creating " + "Lantern_no key " + (i + 1).ToString());
            PlayerPrefs.SetInt("Lantern_no " + (i + 1).ToString(), 0);
        }

        //Makes player object visible and loads first scene
        DontDestroyOnLoad(Player);
        DontDestroyOnLoad(MainCamera);
        SceneManager.LoadScene("SpawnRoom");

        Player.SetActive(true);

        //sets player positon
        Player.transform.position = new Vector3(0, 0, (float)-1.1);


    }

    public void loadNextRoom(int DoorDirection)
    {//loads the next room
        int nextRoom = roomDoors[currentRoom-1, DoorDirection];
    
        GameObject Player = GameObject.FindGameObjectWithTag("Player");
        if (Player == null)
        {
            Debug.Log("Cannot find player object");
        }
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().newRoom();

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
        //resets and hides player, displays respawn scene
        DontDestroyOnLoad(this);
        Player.GetComponent<Player>().resetMe();
        Player.SetActive(false);

        //resets all enemy states to undefeated (1 for defeated, 0 for undefeated)
        for (int i = 0; i < numOfEnemies; i++)
        {
            //Debug.Log("creating " + "Enemy_no key " + (i + 1).ToString());
            PlayerPrefs.SetInt("Enemy_no " + (i + 1).ToString(), 0);
        }
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
