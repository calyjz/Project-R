using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExperiencePointUI : MonoBehaviour
{
    public Text textbox;
    public string exp;
    
    // Start is called before the first frame update
    void Start()
    {
        textbox = GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        exp = GameController.exp.ToString();
        textbox.text = "Experience Points: " + exp;
    }
}
