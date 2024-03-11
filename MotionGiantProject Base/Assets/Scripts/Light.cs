using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Light : MonoBehaviour
{
    private float initialX, initialY, initialZ;
    private Vector3 initialPosition;
    private GameObject darkness;
    private bool run_light;
    public float lightSize = 1f;
    public float lightDecrease = GameController.lightDecrease;
    public GameObject ombre;
    public GameObject winOmbre;

    // Start is called before the first frame update
    void Start()
    {
        initialPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        transform.position = initialPosition;
        initialX = transform.localScale.x;
        initialY = transform.localScale.y;
        initialZ = transform.localScale.z;
        run_light = true;
        
        StartCoroutine("decreaseLightSize");

        //spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    IEnumerator decreaseLightSize()
    {
        if (run_light)
        {
            yield return new WaitForSeconds(lightDecrease);
            if (lightSize > 0.25f)
                lightSize -= 0.005f;
            StartCoroutine("decreaseLightSize");
        }
    }

    // Update is called once per frame
    void Update()
    {   
        transform.localScale = new Vector3(initialX*lightSize, initialY*lightSize, initialZ*lightSize);
        darkness = GameObject.FindGameObjectWithTag("Darkness");
        if (!GameObject.FindGameObjectWithTag("Enemy"))
        {
            ombre.SetActive(false);
            Destroy(darkness);
            run_light = false;
            winOmbre.SetActive(true);
        }
        else
        {
            ombre.SetActive(true);
            run_light = true;
            winOmbre.SetActive(false);
        }
        
       
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

    public void RestartLight()
    {
        StartCoroutine("decreaseLightSize");
    }
    public void NotifyChange()
    {
        lightDecrease = GameController.lightDecrease;
    }
}
