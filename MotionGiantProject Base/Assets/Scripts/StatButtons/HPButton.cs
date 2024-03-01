using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPButton : MonoBehaviour
{
    public static int level = 0;
    private int[] hpLevels = { 100, 125, 150, 175, 200, 225, 250, 275, 300 };
    public void OnButtonPress()
    {
        Debug.Log("Button Clicked");
        if (GameController.exp >= 50 && level < 9)
        {
            level += 1;
            GameController.hp_max = hpLevels[level];
            GameController.exp -= 50;
        }
        
    }
}
