using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Light : MonoBehaviour
{
    private float initialX, initialY, initialZ;
    private Vector3 initialPosition;
    public float lightSize = 1;


   
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = initialPosition;
        initialX = transform.localScale.x;
        initialY = transform.localScale.y;
        initialZ = transform.localScale.z;
        
        StartCoroutine("decreaseLightSize");

        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    IEnumerator decreaseLightSize()
    {
        yield return new WaitForSeconds(0.5F);
        if (lightSize > 0.25f)
            lightSize -= 0.03f;
        StartCoroutine("decreaseLightSize");
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = new Vector3(initialX * lightSize, initialY * lightSize, initialZ * lightSize);
        // Clear the previous hit points
        
    }
    
    public void Pickup()
    {
        if (lightSize + 0.4 > 1)
        {
           lightSize = 1;
        }
        else
        {
            lightSize += 0.4f;
        }
    }
}
