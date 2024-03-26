using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCutsceneIntro : MonoBehaviour
{
    public GameObject[] cutscenes;
    int index;
    // Start is called before the first frame update
    void Start()
    {
        index = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (index >= cutscenes.Length)
        {
            //start game
            Debug.Log(GameController.Instance.GetCurrentRoom());
            GameController.Instance.UpdateGameState(GameState.Run);
        }
        if (index == 0)
        {
            cutscenes[0].gameObject.SetActive(true);
        }
    }
    public void Next()
    {
        
        for(int i=0; i < cutscenes.Length; i++)
        {
            Debug.Log(cutscenes.Length.ToString() + index.ToString());
            cutscenes[i].gameObject.SetActive(false);
            cutscenes[index].gameObject.SetActive(true);
            
        }
        index += 1;
    }
}
