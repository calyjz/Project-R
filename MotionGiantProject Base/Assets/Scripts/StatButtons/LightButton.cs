using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightButton : MonoBehaviour
{
    public static int level = 0;
    private float[] lightLevels = { 0.5f, 0.8f, 1.2f, 1.4f, 1.6f, 1.8f, 2.0f, 2.2f, 2.5f };
    public void OnButtonPress()
    {
        Debug.Log("Button Clicked");
        if (GameController.exp >= 50 && level < 9)
        {
            level += 1;
            GameController.lightDecrease = lightLevels[level];
            GameController.exp -= 50;
        }
        
    }
}
