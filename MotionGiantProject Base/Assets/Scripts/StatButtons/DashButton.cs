using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashButton : MonoBehaviour
{
    public static int level = 0;
    private float[] dashLevels = { 1.00f, 0.85f, 0.75f, 0.65f, 0.6f, 0.55f, 0.5f, 0.45f, 0.4f };
    public void OnButtonPress()
    {
        Debug.Log("Button Clicked");
        if (GameController.exp >= 50 && level < 9)
        {
            level += 1;
            GameController.dashCooldown = dashLevels[level];
            GameController.exp -= 50;
        }
        
    }
}
