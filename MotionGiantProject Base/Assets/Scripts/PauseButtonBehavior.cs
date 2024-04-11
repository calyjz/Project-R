using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButtonBehavior : MonoBehaviour
{

    private bool paused = false;

    public GameObject PauseScreen;
    public GameObject PauseButton;

    // Start is called before the first frame update
    void Start()
    {
        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused == false)//pause the game
            {
                pause();
            }
            else if (paused == true)//unpause the game
            {
                unpause();
            }
        }
        
    }

    public void PauseButtonOnPress()
    {
        //pause the game after pause button gets pressed
        pause();
    }

    public void ResumeButtonOnPress()
    {
        //unpause the game after respawn button gets pressed
        unpause();
    }
    
    public void RestartButtonOnPress()
    {
        //restart the entire game
        Time.timeScale = 1f;
        GameController.Instance.GameReset();
    }

    public void pause()
    {
        //pause the game
        paused = true;
        PauseScreen.SetActive(true);
        PauseButton.SetActive(false);
        Debug.Log("Paused");
        Time.timeScale = 0f;
    }

    public void unpause()
    {
        //unpause the game
        paused = false;
        PauseScreen.SetActive(false);
        PauseButton.SetActive(true);
        Debug.Log("Unpaused");
        Time.timeScale = 1f;
    }
}
