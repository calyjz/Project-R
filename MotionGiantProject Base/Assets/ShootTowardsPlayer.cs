using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootTowardsPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 startPos;
    Vector3 endPos;
    
    private Vector3 movingDir;

    private float startTime;
    public float speed = 1.0f;
    private float journeyLength;

    public LayerMask enemiesLayer;
    private float totalTime = 0f;
    void Start()
    {
        startPos = transform.position;
        movingDir = (GameObject.FindGameObjectWithTag("Player").transform.position- transform.position).normalized * 10f;
        journeyLength = Vector3.Distance(startPos, endPos);
        startTime = Time.time;
    }


    //IEnumerator LerpFunction(Vector3 targePosition)
    //{
    //    float time = 0;
    //    Vector3 startPosition = transform.position;
    //    while (time < duration)
    //    {
    //        transform.position = Vector3.Lerp(startPos, endPos,)
    //        time += Time.deltaTime;
    //    }
    //}
    // Update is called once per frame
    void Update()
    {
        float distCovered = (Time.time - startTime + totalTime) * speed;
        float fractionOfJourney = distCovered / journeyLength;
        if (fractionOfJourney > 0.95f)
        {
            Destroy(gameObject);
        }
        distCovered = (Time.time - startTime) * speed;
        transform.position = startPos+distCovered* movingDir;

        Collider2D[] enemiesSelf = Physics2D.OverlapCircleAll(transform.position, 0.12f, enemiesLayer);

        //// Reflect the bullet's direction if it hits an obstacle
        if (enemiesSelf.Length > 0)
        {
            for (int i = 0; i < enemiesSelf.Length; i++)
            {
                
                if(fractionOfJourney>0.035f)
                {
                    Destroy(enemiesSelf[i].gameObject);
                    Destroy(gameObject);
                }
                
            }
                //    // Get the normal of the first obstacle hit
                ///    Vector2 normal = (transform.position - obstacles[0].transform.position).normalized;

                //    // Reflect the direction
                //    movingDir = Vector2.Reflect(movingDir, normal);
        }
    }

    public void deflect(Vector3 normal)
    {
        movingDir = Vector2.Reflect(movingDir, normal);
        startPos = transform.position;
        totalTime += Time.time - startTime;
        startTime = Time.time-0.05f;
    }
    //void CollisionEnter2D(Collision2D collide) {
    //    Debug.Log(collide);
    //    movingDir = Vector3.Reflect(movingDir, collide.contacts[0].normal);
    //}
}
