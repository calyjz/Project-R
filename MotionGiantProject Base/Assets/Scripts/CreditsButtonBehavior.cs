using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonBehavior : MonoBehaviour
{
    public GameObject Canvas;
    public void OnButtonPress()
    {
        SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
        Debug.Log("Button Clicked");




        Canvas.SetActive(true);



    }

    void Start()
    {
        Canvas.SetActive(false);
    }
}
