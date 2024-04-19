using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lantern : AnimatedLantern
{
    public int lantern_no;

    public void Start()
    {
        AnimationSetup();
    }

    public void RemoveLantern()
    {   //check if player is not dead
        //check if the enemy wasnt defeated before (store a value inside a file associated with lantern#. 1 for removed, 0 for still present)
        if (PlayerPrefs.GetInt("Lantern_no " + lantern_no) == 0)
        {

            //set lantern to removed status
            PlayerPrefs.SetInt("Lantern_no " + lantern_no, 1);
            Debug.Log("setting" + lantern_no.ToString() + " to " + PlayerPrefs.GetInt("Enemy_no " + lantern_no));
        }
        
    }

    private void Update()
    {
        AnimationUpdate();

        if (PlayerPrefs.GetInt("Lantern_no " + lantern_no) == 1)
        {
            Destroy(this.gameObject);
        }
    }
}
