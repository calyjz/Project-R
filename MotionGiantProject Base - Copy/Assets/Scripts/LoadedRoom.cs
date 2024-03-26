using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadedRoom : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().newRoom();
        //this does not work?
        GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>().RestartLight();
    }

    
}
