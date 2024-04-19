using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsDisplay : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.firstrun != true)
        {
            Destroy(gameObject);
        }
        
    }
}
