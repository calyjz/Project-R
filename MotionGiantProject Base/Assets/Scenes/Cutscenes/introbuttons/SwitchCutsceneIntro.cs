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

        for (int i = 1; i < cutscenes.Length; i++)
        {
            cutscenes[i].gameObject.SetActive(false);
        }
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

        
        Debug.Log(index.ToString() + " false, " + (index+1).ToString() + " true");
        cutscenes[index].gameObject.SetActive(false);
        index += 1;
        if (index < cutscenes.Length)
        {
            cutscenes[index].gameObject.SetActive(true);
        }
        
    }
}
