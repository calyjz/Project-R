using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIButtonBehavior : MonoBehaviour
{
    [Header("HP")]
    public int HPCost = 50;
    public int HPIncrease = 25;
    public int HPMax = 300; 
    private int HPClicks = 0;
    public Text textHP;
    public Button HPButtonUP;
    public Button HPButtonDOWN;

    [Header("Dash")]
    public int DashCost = 50;
    public float DashMax = 0.5f;
    public float DashIncrease = 0.05f;
    private int DashClicks = 0;
    public Text textDash;
    public Button DashButtonUP;
    public Button DashButtonDOWN;

    [Header("Attack")]
    public int AttackCost = 50;
    public int AttackIncrease = 5;
    public int AttackMax = 100;
    private int Attack;
    private int AttackClicks = 0;
    public Text textAttack;
    public Button AttackButtonUP;
    public Button AttackButtonDOWN;

    [Header("Light")]
    public int LightCost = 50;
    public float LightIncrease = 0.3f;
    public float LightMax = 2.9f;
    private float Light;
    private int LightClicks = 0;
    public Text textLight;
    public Button LightButtonUP;
    public Button LightButtonDOWN;


    private string exp;
    private static Player playerObject;
    private static Light lightObject;
    private int dashClicks, attackClicks, lanternClicks;

    public int points = 100;
   

    public void hpUP()
    {
        if (GameController.exp >= HPCost && GameController.hp_max < HPMax)//increase stat if user has enough exp
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.hp_max += HPIncrease;
            HPClicks += 1;
            GameController.exp -= HPCost;
            textHP.text = GameController.hp_max.ToString();
        }
        Debug.Log(HPClicks);
    }
    public void hpDOWN()
    {
        if (HPClicks > 0)//checks if the user already clicked  the up button previously
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.hp_max -= HPIncrease;
            HPClicks -= 1;
            GameController.exp += HPCost;
            textHP.text = GameController.hp_max.ToString();
        }
       
    }

    public void dashUP()
    {
        Debug.Log(DashIncrease);
        if (GameController.exp >= DashCost && GameController.dashCooldown > DashMax)//increase stat if user has enough exp
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
       
            GameController.dashCooldown -= DashIncrease;//subtracts the decrease in cooldown time from gamecontroller stat
            DashClicks += 1;
            GameController.exp -= DashCost;
            textDash.text = (1-GameController.dashCooldown).ToString();
        }
    }
    public void dashDOWN()
    {
        if (DashClicks > 0)//checks if the user already clicked the up button previously
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.dashCooldown += DashIncrease;//increase cooldown
            DashClicks -= 1;
            GameController.exp += DashCost;
            textDash.text = (1-GameController.dashCooldown).ToString();
        }

    }

    public void attackUP()
    {
        Debug.Log(AttackIncrease);
        if (GameController.exp >= AttackCost && GameController.attackPower < AttackMax)//increase stat if user has enough exp
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.attackPower += AttackIncrease;//increases attack
            AttackClicks += 1;
            GameController.exp -= AttackCost;
            textAttack.text = GameController.attackPower.ToString();
        }
    }
    public void attackDOWN()
    {
        if (AttackClicks > 0)//checks if the user already clicked the up button previously
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.attackPower -= AttackIncrease;//increase cooldown
            AttackClicks -= 1;
            GameController.exp += AttackCost;
            textAttack.text = GameController.attackPower.ToString();
        }
    }



    public void LanternUP()
    {
        if (GameController.exp >= LightCost && GameController.lightDecrease < LightMax)//increase stat if user has enough exp
        {
            
            GameController.lightDecrease += LightIncrease;//Adds to light decrease
            LightClicks += 1;
            GameController.exp -= LightCost;
            textLight.text = GameController.lightDecrease.ToString();
        }
    }
    public void LanternDOWN()
    {
        if (LightClicks > 0)//checks if the user already clicked the up button previously
        {
            GameController.lightDecrease -= LightIncrease;//increase cooldown
            LightClicks -= 1;
            GameController.exp += LightCost;
            textLight.text = GameController.lightDecrease.ToString();
        }

    }

    void Start()
    {
        textHP.text = GameController.hp_max.ToString();
        textDash.text = (1- GameController.dashCooldown).ToString();
        textAttack.text = GameController.attackPower.ToString();
        textLight.text = GameController.lightDecrease.ToString();
        //textbox = GetComponent<Text>();
        //playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        Debug.Log(DashIncrease);
    }

    private void Update()
    {

        if (HPClicks == 0) //disables down button if no points have been allocated
        {
            HPButtonDOWN.interactable = false;
        }
        else
        {
            HPButtonDOWN.interactable = true;
        }
        if (GameController.exp < HPCost)//disables up button if no more points can be allocated
        {
            HPButtonUP.interactable = false;
        }
        else
        {
            HPButtonUP.interactable = true;
        }
        //--------------------
        if (DashClicks == 0) //disables down button if no points have been allocated
        {
            DashButtonDOWN.interactable = false;
        }
        else
        {
            DashButtonDOWN.interactable = true;
        }

        if (GameController.exp < DashCost)//disables up button if no more points can be allocated
        {
            DashButtonUP.interactable = false;
        }
        else
        {
            DashButtonUP.interactable = true;
        }
        
        //-----------------
        if (AttackClicks == 0) //disables down button if no points have been allocated
        {
            AttackButtonDOWN.interactable = false;
        }
        else
        {
            AttackButtonDOWN.interactable = true;
        }
        if (GameController.exp < AttackCost)//disables up button if no more points can be allocated
        {
            AttackButtonUP.interactable = false;
        }
        else
        {
            AttackButtonUP.interactable = true;
        }



        //-----------------
        if (LightClicks == 0) //disables down button if no points have been allocated
        {
            LightButtonDOWN.interactable = false;
        }
        else
        {
            LightButtonDOWN.interactable = true;
        }
        if (GameController.exp < LightCost)//disables up button if no more points can be allocated
        {
            LightButtonUP.interactable = false;
        }
        else
        {
            LightButtonUP.interactable = true;
        }
    }

}
