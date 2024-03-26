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
    //public GameObject winOmbre;

    private float ColorTimer = 0;
    private string state;
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
        if (ColorTimer > 0)
        {
            ColorTimer -= Time.deltaTime;
        }
        //Debug.Log(ColorTimer);
        //Color newColor = Color.white;
        //newColor.a = ColorTimer;
        //ombre.GetComponent<OmbreArt>().ChangeAlpha(1-ColorTimer);
        //////Color newColor2 = Color.white;
        //newColor2.a = 1 - ColorTimer;
        //winOmbre.GetComponent<OmbreArt>().ChangeAlpha(ColorTimer);

        //winOmbre.GetComponent<Renderer>().sharedMaterial.SetColor("Tint", newColor2);


        transform.localScale = new Vector3(initialX*lightSize, initialY*lightSize, initialZ*lightSize);
        darkness = GameObject.FindGameObjectWithTag("Darkness");
        if (!GameObject.FindGameObjectWithTag("Enemy"))
        {
            ombre.SetActive(false);
            Destroy(darkness);
            run_light = false;
            //if(state != "noEnemiesLeft")
            //{
            //ColorTimer = 1;

            //}
            //state = "noEnemiesLeft";
            //winOmbre.SetActive(true);
        }
        else
        {
            //ColorTimer = 0;
            ombre.SetActive(true);
            run_light = true;
            //state = "killEnemies";
            //winOmbre.SetActive(false);
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
        run_light = true;
        Debug.Log("FLSKJFSDLKFJSDLKFJSDLKFJ");
        StartCoroutine("decreaseLightSize");
    }
    public void NotifyChange()
    {
        lightDecrease = GameController.lightDecrease;
    }
}
