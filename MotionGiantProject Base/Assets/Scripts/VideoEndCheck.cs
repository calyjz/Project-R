using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class VideoEndCheck : MonoBehaviour
{
    [SerializeField] public VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer.loopPointReached += nextScene;
    }

    public void nextScene(VideoPlayer vp)
    {
        if (SceneManager.GetActiveScene().name == "Intro")
        {
            GameController.Instance.UpdateGameState(GameState.Run);
        } else if (SceneManager.GetActiveScene().name == "Outro")
        {
            GameController.Instance.GameReset();
        }
    }
}
