using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pit : MonoBehaviour
{
    private void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 9, true);
        Physics2D.IgnoreLayerCollision(12, 8, false);
    }
}
