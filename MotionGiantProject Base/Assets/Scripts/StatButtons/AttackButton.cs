using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackButton : MonoBehaviour
{
    public static int level = 0;
    private int[] attackLevels = { 1, 2, 3, 4, 5, 6, 7, 8, 10 };
    public void OnButtonPress()
    {
        SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
        Debug.Log("Button Clicked");
        if (GameController.exp >= 50 && level < 9)
        {
            level += 1;
            GameController.attackPower = attackLevels[level];
            GameController.exp -= 50;
        }
        
    }
}
