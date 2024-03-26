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
        if (index > cutscenes.Length)
        {
            //start game
        }
        if (index == 0)
        {
            cutscenes[0].gameObject.SetActive(true);
        }
    }
    public void Next()
    {
        index += 1;
        for(int i=0; i < cutscenes.Length; i++)
        {
            cutscenes[i].gameObject.SetActive(false);
            cutscenes[index].gameObject.SetActive(true);
        }
    }
}
