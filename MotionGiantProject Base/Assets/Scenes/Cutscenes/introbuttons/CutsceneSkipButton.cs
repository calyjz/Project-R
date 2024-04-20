using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CutsceneSkipButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Skip()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name == "Outro")
        {
            Time.timeScale = 1f;
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.Instance.GameReset();
        }

        if (SceneManager.GetActiveScene().name == "Intro")
        {
            Time.timeScale = 1f;
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.Instance.UpdateGameState(GameState.Run);
        }
    }
}
