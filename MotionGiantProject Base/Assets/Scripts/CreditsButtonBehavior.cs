using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsButtonBehavior : MonoBehaviour
{
    public void OnButtonPress()
    {
        SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
        Debug.Log("Button Clicked");


       

            GameController.Instance.Credits();



    }
}
