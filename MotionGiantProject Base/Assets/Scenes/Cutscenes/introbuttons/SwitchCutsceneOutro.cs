using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwitchCutsceneOutro : MonoBehaviour
{
    public GameObject[] cutscenes;
    int index;
    public GameObject Player;
    public GameObject MainCamera;
    // Start is called before the first frame update
    void Start()
    {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        Player = GameObject.FindGameObjectWithTag("Player");
        Destroy(MainCamera);
        Destroy(Player);
        
        index = 0;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (index > cutscenes.Length)
        {
            index = 12;
        }
        if (index == 0)
        {
            cutscenes[0].gameObject.SetActive(true);
        }
    }
    public void Next()
    {
        if (index >= cutscenes.Length)
        {
            index = 12;
        }

        for (int i=0; i < cutscenes.Length; i++)
        {
            cutscenes[i].gameObject.SetActive(false);
            cutscenes[index].gameObject.SetActive(true);
        }
        index += 1;


        Debug.Log(index.ToString() + cutscenes.Length.ToString());
    }
}
