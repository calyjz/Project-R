using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartRun : MonoBehaviour
{
    private void Awake()
    {
        GameController.Instance.Player = GameObject.FindGameObjectWithTag("Player");
        GameController.Instance.MainCamera = GameObject.FindGameObjectWithTag("MainCamera");

        DontDestroyOnLoad(GameController.Instance.Player);
        DontDestroyOnLoad(GameController.Instance.MainCamera);

    }
}
