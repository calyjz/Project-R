using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButtonBehavior : MonoBehaviour
{
    public void OnButtonPress()
    {
        Debug.Log("Button Clicked");
        if (tag.Contains("Respawn"))
        {
            MusicManager.instance.ResumeMusic();
        }
        GameController.Instance.UpdateGameState(GameState.Run);
        
    }
}
