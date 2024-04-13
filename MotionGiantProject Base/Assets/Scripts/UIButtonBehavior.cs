using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIButtonBehavior : MonoBehaviour
{
    [Header("HP")]
    public int HPCost = 75;
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
    public int AttackCost = 200;
    public int AttackIncrease = 50;
    public int AttackMax = 135;
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


    private static Player playerObject;
    private static Light lightObject;
   

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
        if (GameController.exp >= DashCost && 0.6f - GameController.dashCooldown <= DashMax)//increase stat if user has enough exp
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
       
            GameController.dashCooldown -= DashIncrease;//subtracts the decrease in cooldown time from gamecontroller stat
            DashClicks += 1;
            GameController.exp -= DashCost;
            textDash.text = (0.6f-GameController.dashCooldown).ToString("F2");
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
            textDash.text = (0.6f-GameController.dashCooldown).ToString("F2");
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
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.lightDecrease += LightIncrease;//Adds to light decrease
            LightClicks += 1;
            GameController.exp -= LightCost;
            textLight.text = GameController.lightDecrease.ToString("F1");
        }
    }
    public void LanternDOWN()
    {
        if (LightClicks > 0)//checks if the user already clicked the up button previously
        {
            SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
            GameController.lightDecrease -= LightIncrease;//increase cooldown
            LightClicks -= 1;
            GameController.exp += LightCost;
            textLight.text = GameController.lightDecrease.ToString("F1");
        }

    }

    void Start()
    {
        textHP.text = GameController.hp_max.ToString();
        textDash.text = (0.6f- GameController.dashCooldown).ToString("F2");
        textAttack.text = GameController.attackPower.ToString();
        textLight.text = GameController.lightDecrease.ToString("F1");
        //textbox = GetComponent<Text>();
        //playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void RespawnPress()
    {
        SoundFXManager.instance.PlaySoundFXClip("ButtonPress", this.transform);
        Debug.Log("Button Clicked");
        MusicManager.instance.PlayNextTrack();
        

        GameController.Instance.UpdateGameState(GameState.Run);


        //GameController.Instance.UpdateGameState(GameState.Run);

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
        if (GameController.exp < HPCost || GameController.hp_max >= HPMax)//disables up button if no more points can be allocated
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

        if (GameController.exp < DashCost || 0.6f - GameController.dashCooldown > DashMax)//disables up button if no more points can be allocated
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
        if (GameController.exp < AttackCost || GameController.attackPower >= AttackMax)//disables up button if no more points can be allocated
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
        if (GameController.exp < LightCost || GameController.lightDecrease >= LightMax)//disables up button if no more points can be allocated
        {
            LightButtonUP.interactable = false;
        }
        else
        {
            LightButtonUP.interactable = true;
        }
    }

}
