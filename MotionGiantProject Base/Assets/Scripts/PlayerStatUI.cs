using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.UI;
using UnityEngine.UI;

public class PlayerStatUI : MonoBehaviour
{
    public Text textbox;
    public string hp, dashCooldown, dashCurrent, attackPower, lanternTime;
    private static Player playerObject;
    private static Light lightObject;
    
    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponent<Text>();
        playerObject = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        lightObject = GameObject.FindGameObjectWithTag("LightObject").GetComponent<Light>();

    }

    // Update is called once per frame
    void Update()
    {
        hp = playerObject.HP.ToString();
        dashCurrent = Math.Round(playerObject.getDashCoolCurrent(), 2).ToString();
        dashCooldown = GameController.dashCooldown.ToString();
        attackPower = GameController.attackPower.ToString();
        lanternTime = GameController.lightDecrease.ToString();
        textbox.text = hp + "\n" + dashCurrent + " / " + dashCooldown + "\n" + attackPower + " AP\n-" + lanternTime + "s";
    }
}
